using Microsoft.EntityFrameworkCore;
using RA.DAL.Exceptions;
using RA.Database;
using RA.Database.Models;
using RA.DTO;
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
        public async Task<ClockDto> GetClock(int id)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var result = await dbContext.Clocks.Where(c => c.Id == id)
                .Select(c => ClockDto.FromEntity(c))
                .FirstOrDefaultAsync();
            if(result == null)
            {
                throw new NotFoundException($"Clock with id {id} not found");
            }
            return result;
        }
        public IEnumerable<ClockItemDto> GetClockItems(int clockId)
        {
            return GetClockItemsAsync(clockId).Result;
        }
        public async Task<IEnumerable<ClockItemDto>> GetClockItemsAsync(int clockId)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            return await dbContext.ClockItems
                .Where(cl => cl.ClockId == clockId)
                .Include(c => c.Category)
                .OrderBy(ci => ci.OrderIndex)
                .Select(ci => ClockItemDto.FromEntity(ci))
                .ToListAsync();
        }
        public IEnumerable<ClockDto> GetClocks()
        {
            return GetClocksAsync().Result;
        }
        public async Task<IEnumerable<ClockDto>> GetClocksAsync()
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            return await dbContext.Clocks
                .Select(c => ClockDto.FromEntity(c))
                .ToListAsync();
        }
        public async Task<Dictionary<int, TimeSpan>> CalculateAverageDurationsForCategoriesInClockWithId(int clockId)
        {
            Dictionary<int, TimeSpan> result = new Dictionary<int, TimeSpan>();
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                // Get unique ids for current clockItemDto items
                var categoryIds = await dbContext.ClockItems
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
        public async Task AddClock(ClockDto clockDto)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var entity = ClockDto.ToEntity(clockDto);
            await dbContext.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateClock(ClockDto clockDto)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var entity = ClockDto.ToEntity(clockDto);
            dbContext.Update(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}
