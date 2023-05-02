using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RA.Database;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public class DefaultSchedulesService : IDefaultSchedulesService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;

        public DefaultSchedulesService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<IDictionary<DateTime, ScheduleDefaultItemDto?>> GetDefaultSchedulesOverviewAsync(DateTime searchDateStart, DateTime searchDateEnd)
        {
            var dictionary = new Dictionary<DateTime, ScheduleDefaultItemDto?>();
            using var dbContext = dbContextFactory.CreateDbContext();

            var defaultSchedulesInRange = await dbContext.SchedulesDefault
                .Include(item => item.ScheduleDefaultItems)
                .ThenInclude(item => item.Template)
                 .Where(item =>
                       (item.StartDate <= searchDateEnd) &&
                       (item.EndDate >= searchDateStart)
                       )
                .OrderBy(item => item.StartDate)
                .ThenBy(item => item.EndDate)
                .Select(item => ScheduleDefaultDto.FromEntity(item))
                .ToListAsync();

            DateTime dateIndex = searchDateStart;
            while (dateIndex <= searchDateEnd)
            {
                DayOfWeek day = dateIndex.DayOfWeek;
                var schedule = defaultSchedulesInRange.Where(schedule => (schedule.StartDate <= searchDateEnd) 
                                                                            && (schedule.EndDate >= searchDateStart))
                                                                            .FirstOrDefault();
                var scheduleItem = schedule?.Items?.Where(s => s.DayOfWeek == day).FirstOrDefault();
                if(schedule == null || scheduleItem == null)
                {
                    //There is no schedule found for this day, so we send an empty one
                    dictionary[dateIndex] = null;
                } else
                {
                    dictionary[dateIndex] = scheduleItem;
                }
                   
                dateIndex = dateIndex.AddDays(1);
            }
            return dictionary;
        }

        public IDictionary<DateTime, ScheduleDefaultItemDto?> GetDefaultSchedulesOverview(DateTime searchDateStart, DateTime searchDateEnd)
        {
            return GetDefaultSchedulesOverviewAsync(searchDateStart, searchDateEnd).Result;
        }
        public IEnumerable<ScheduleDefaultDto> GetDefaultSchedules(int skip = 0, int limit = 100, bool ascending = false)
        {
            throw new NotImplementedException();
        }
    }
}
