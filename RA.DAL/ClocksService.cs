using Castle.Core.Smtp;
using Microsoft.EntityFrameworkCore;
using RA.DAL.Exceptions;
using RA.Database;
using RA.Database.Models;
using RA.DTO;
using RA.DTO.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if(result == null)
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
                .Include(ci => ((ClockItemTrack)ci).Track)
                .ThenInclude(ci => ci.TrackArtists)
                .ThenInclude(ta => ta.Artist)
                .ToListAsync()
                ;

           foreach(var item in clockItems)
            {
                if(item is ClockItemCategory itemCategory)
                {
                    result.Add(ClockItemCategoryDTO.FromEntity(itemCategory));
                } else if(item is ClockItemTrack itemTrack)
                {
                    result.Add(ClockItemTrackDTO.FromEntity(itemTrack));
                }
                else if(item is ClockItemEvent itemEvent)
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
                    var categoryAvgDurations = dbContext.Categories
                        .Where(c => categoryIds.Contains(c.Id) || categoryIds.Contains(c.ParentId))
                        .Select(c => new
                        {
                            CategoryId = c.Id,
                            ParentId = c.ParentId,
                            AvgDuration = c.Tracks.Average(t => (double?)t.Duration)
                        })
                        .ToList();

                    foreach (var item in categoryAvgDurations)
                    {
                        TimeSpan avgDuration = TimeSpan.Zero;
                        if (item.AvgDuration.HasValue)
                        {
                            avgDuration = TimeSpan.FromSeconds(item.AvgDuration.Value);
                        }

                        if (item.ParentId == null)
                        {
                            // If the category is a parent, calculate the average duration of its subcategories.
                            var subCategories = categoryAvgDurations.Where(x => x.ParentId == item.CategoryId).ToList();
                            TimeSpan totalDuration = avgDuration;
                            int count = 1; // 1 to include the parent itself

                            foreach (var subItem in subCategories)
                            {
                                totalDuration += TimeSpan.FromSeconds(subItem.AvgDuration ?? 0);
                                count++;
                            }
                            avgDuration = totalDuration / count;
                        }

                        result.Add(item.CategoryId, avgDuration);
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
            dbContext.Add(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<ClockItemBaseDTO> GetClockItemAsync(int clockItemId)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var entity = await dbContext.ClockItems.Where(ci => ci.Id == clockItemId)
                .FirstOrDefaultAsync();
            if(entity == null)
            {
                throw new NotFoundException($"Clock item with id {clockItemId} does not exist!");
            }
            return ClockItemBaseDTO.FromEntity(entity);
            
        }

        public async Task RemoveClockItem(ClockItemBaseDTO clockItemDto)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var entity = ClockItemBaseDTO.ToEntity(clockItemDto);
            dbContext.Remove(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}
