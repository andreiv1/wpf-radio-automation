using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using RA.DAL.Exceptions;
using RA.Database;
using RA.Database.Models;
using RA.Database.Models.Abstract;
using RA.DTO;
using RA.DTO.Abstract;
using System.Diagnostics;

namespace RA.DAL
{
    public class ClocksService : IClocksService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;
        public ClocksService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }
        public async Task<ClockDTO> GetClock(int id)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var result = await dbContext.Clocks.Where(c => c.Id == id)
                .Select(c => ClockDTO.FromEntity(c))
                .FirstOrDefaultAsync();
            if (result == null)
            {
                throw new NotFoundException($"Clock with id {id} not found");
            }
            return result;
        }
        public IEnumerable<ClockItemBaseDTO> GetClockItems(int clockId)
        {
            return GetClockItemsAsync(clockId).Result;
        }
        public async Task<IEnumerable<ClockItemBaseDTO>> GetClockItemsAsync(int clockId)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var result = new List<ClockItemBaseDTO>();

            var clockItems = await dbContext.ClockItems
                .Where(ci => ci.ClockId == clockId)
                .OrderBy(ci => ci.OrderIndex)
                .Include(ci => ((ClockItemCategory)ci).Category)
                .Include(ci => ((ClockItemCategory)ci).ClockItemCategoryTags)
                .ThenInclude(t => ((ClockItemCategoryTag)t).TagValue.TagCategory)
                .Include(ci => ((ClockItemTrack)ci).Track)
                .ThenInclude(ci => ci.TrackArtists)
                .ThenInclude(ta => ta.Artist)
                .IgnoreQueryFilters()
                .AsNoTracking()
                .ToListAsync()
                ;

            foreach (var item in clockItems)
            {
                if (item is ClockItemCategory itemCategory)
                {
                    result.Add(ClockItemCategoryDTO.FromEntity(itemCategory));
                }
                else if (item is ClockItemTrack itemTrack)
                {
                    result.Add(ClockItemTrackDTO.FromEntity(itemTrack));
                }
                else if (item is ClockItemEvent itemEvent)
                {
                    result.Add(ClockItemEventDTO.FromEntity(itemEvent));
                }

            }

            return result;
        }
        public IEnumerable<ClockDTO> GetClocks()
        {
            return GetClocksAsync().Result;
        }
        public async Task<IEnumerable<ClockDTO>> GetClocksAsync()
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            return await dbContext.Clocks
                .Select(c => ClockDTO.FromEntity(c))
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Dictionary<int, TimeSpan>> CalculateAverageDurationsForCategoriesInClockWithId(int clockId)
        {
            Dictionary<int, TimeSpan> result = new Dictionary<int, TimeSpan>();
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                // Get unique ids for current clockItemDto items
                var categoryIds = await dbContext.ClockItemsCategories
                    .Where(ci => ci.ClockId == clockId)
                    .Select(ci => ci.CategoryId)
                    .Distinct().ToListAsync();

                if (categoryIds.Count > 0)
                {
                    foreach (var id in categoryIds)
                    {
                        TimeSpan avgDuration = TimeSpan.Zero;
                        if (id != null)
                        {
                            avgDuration = await dbContext.GetCategoryAvgDuration(id.Value);
                            result.Add(id.Value, avgDuration);
                        }
                    }

                }
            }
            return result;
        }
        public async Task AddClock(ClockDTO clockDto)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var entity = ClockDTO.ToEntity(clockDto);
            await dbContext.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateClock(ClockDTO clockDto)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var entity = ClockDTO.ToEntity(clockDto);
            dbContext.Update(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task AddClockItem(ClockItemBaseDTO clockItemDto)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var entity = ClockItemBaseDTO.ToEntity(clockItemDto);
            dbContext.ClockItems.Add(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateClockItem(ClockItemCategoryDTO clockItemCategoryDto)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var existingClockItem = dbContext.ClockItems
                .Where(ci => ci.Id == clockItemCategoryDto.Id)
                .Include(ci => ((ClockItemCategory)ci).ClockItemCategoryTags)
                .FirstOrDefault() as ClockItemCategory;
            var clockItem = ClockItemCategoryDTO.ToEntity(clockItemCategoryDto);
            if (existingClockItem == null) return;

            dbContext.Entry(existingClockItem).CurrentValues.SetValues(clockItem);


            if (clockItemCategoryDto.Tags != null)
            {
                // Remove tags no longer associated
                foreach (var existingTag in existingClockItem.ClockItemCategoryTags.ToList())
                {
                    if (!clockItemCategoryDto.Tags
                        .Any(t => t.TagValueId == existingTag.TagValueId))
                    {
                        existingClockItem.ClockItemCategoryTags.Remove(existingTag);
                    }

                }

                // Add new tags
                foreach (var newTag in clockItem.ClockItemCategoryTags)
                {
                    if (!existingClockItem.ClockItemCategoryTags
                        .Any(t => t.TagValueId == newTag.TagValueId))
                    {
                        existingClockItem.ClockItemCategoryTags.Add(newTag);
                    }
                }
            }


            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteClockItem(int clockItemId)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var entity = await dbContext.ClockItems.FindAsync(clockItemId);
            if (entity != null)
            {
                dbContext.Remove(entity);
                await dbContext.SaveChangesAsync();


                if (entity.OrderIndex != -1)
                {
                    var otherNormalItems = await dbContext.ClockItems
                        .Where(ci => ci.ClockId == entity.ClockId && ci.OrderIndex > -1)
                        .OrderBy(ci => ci.OrderIndex)
                        .ToListAsync();
                    for (int i = 0; i < otherNormalItems.Count(); i++)
                    {
                        var itm = otherNormalItems.ElementAt(i);
                        itm.OrderIndex = i;
                        dbContext.Update(itm);
                    }
                }
                else if (entity.OrderIndex == -1 && entity.ClockItemEventId != null)
                {
                    var otherEventItems = await dbContext.ClockItems
                        .Where(ci => ci.OrderIndex == -1 && ci.ClockItemEventId == entity.ClockItemEventId)
                        .OrderBy(ci => ci.EventOrderIndex)
                        .ToListAsync();
                    for (int i = 0; i < otherEventItems.Count(); i++)
                    {
                        var itm = otherEventItems.ElementAt(i);
                        itm.EventOrderIndex = i;
                        dbContext.Update(itm);
                    }
                }

                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DuplicateClockItems(ICollection<int> clockItemsIds, int clockId)
        {
            using var dbContext = dbContextFactory.CreateDbContext();


            var clockItems = await dbContext.ClockItems
                //.Include(ci => ci.)
                .Where(ci => ci.OrderIndex > -1) //todo: handle also event items + subitems
                .AsNoTracking()
                .ToListAsync();

            var lastItem = clockItems.LastOrDefault();
            int maxOrderIndex = 0;
            if (lastItem != null)
            {
                maxOrderIndex = lastItem.OrderIndex;
            }

            var itemsToDuplicate = clockItems.Where(ci => clockItemsIds.Contains(ci.Id)).ToList();
            List<ClockItemBase> duplicatedItems = new();
            foreach (var item in itemsToDuplicate)
            {
                if (item is ClockItemCategory itmCategory)
                {
                    duplicatedItems.Add(new ClockItemCategory
                    {
                        OrderIndex = ++maxOrderIndex,
                        ClockId = itmCategory.ClockId,
                        CategoryId = itmCategory.CategoryId,
                        MinDuration = itmCategory.MinDuration,
                        MaxDuration = itmCategory.MaxDuration,
                        ArtistSeparation = itmCategory.ArtistSeparation,
                        TitleSeparation = itmCategory.TitleSeparation,
                        TrackSeparation = itmCategory.TrackSeparation,
                        MinReleaseDate = itmCategory.MinReleaseDate,
                        MaxReleaseDate = itmCategory.MaxReleaseDate,
                        IsFiller = itmCategory.IsFiller,
                        //ClockItemCategoryTags = itmCategory.ClockItemCategoryTags.Select(t => new ClockItemCategoryTag
                        //{
                        //    TagValueId = t.TagValueId
                        //}).ToList()

                    });

                }
                else if (item is ClockItemTrack itmTrack)
                {
                    duplicatedItems.Add(new ClockItemTrack
                    {
                        OrderIndex = ++maxOrderIndex,
                        ClockId = itmTrack.ClockId,
                        TrackId = itmTrack.TrackId,

                    });
                }

            }


            await dbContext.ClockItems.AddRangeAsync(duplicatedItems);
            await dbContext.SaveChangesAsync();
        }

        public async Task<ClockItemBaseDTO> GetClockItemAsync(int clockItemId)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var entity = await dbContext.ClockItems
                .Where(ci => ci.Id == clockItemId)
                .Include(ci => ((ClockItemCategory)ci).ClockItemCategoryTags)
                .ThenInclude(t => ((ClockItemCategoryTag)t).TagValue.TagCategory)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (entity == null)
            {
                throw new NotFoundException($"Clock item with id {clockItemId} does not exist!");
            }
            return ClockItemBaseDTO.FromEntity(entity);

        }

        public async Task<bool> RemoveClock(int clockId)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var isDeleted = false;
            try
            {
                if ((await dbContext.Clocks
                    .Where(c => c.Id == clockId)
                    .ExecuteDeleteAsync()) == 1)
                {
                    isDeleted = true;
                }
            }
            catch (MySqlException e)
            {
                isDeleted = false;
                Debug.WriteLine($"Error deleting ClockId={clockId}: {e.Message}");
            }

            return isDeleted;
        }
    }
}
