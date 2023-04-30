using Microsoft.EntityFrameworkCore;
using RA.Database;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public class ScheduleService : IScheduleService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;

        public ScheduleService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public IDictionary<DateTime, DefaultScheduleDto> GetDefaultScheduleOverview(DateTime searchDateStart, DateTime searchDateEnd)
        {
            return GetDefaultScheduleOverviewAsync(searchDateStart, searchDateEnd).Result;
        }

        public async Task<IDictionary<DateTime, DefaultScheduleDto>> GetDefaultScheduleOverviewAsync(DateTime searchDateStart, DateTime searchDateEnd)
        {
            var dictionary = new Dictionary<DateTime, DefaultScheduleDto>();
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var defaultSchedules = await dbContext.DefaultSchedules
                    .Include(ds => ds.Template)
                    .Where(ds =>
                        (ds.StartDate <= searchDateEnd) &&
                        (!ds.EndDate.HasValue || ds.EndDate >= searchDateStart))
                    .OrderBy(ds => ds.StartDate)
                    .OrderBy(ds => ds.EndDate)
                    .ToListAsync();

                DateTime dateIndex = searchDateStart;
                while (dateIndex <= searchDateEnd)
                {
                    DayOfWeek day = dateIndex.DayOfWeek;
                    var item = defaultSchedules.Where(ds => ds.DayOfWeek == day &&
                        (ds.StartDate <= searchDateEnd) &&
                        (!ds.EndDate.HasValue || ds.EndDate >= dateIndex))
                        .FirstOrDefault();

                    if (item != null)
                    {
                        dictionary[dateIndex] = DefaultScheduleDto.FromEntity(item);
                    }
                    else
                    {
                        //There is no schedule found for this day, so we send an empty schedule
                        dictionary[dateIndex] = new DefaultScheduleDto();
                    }
                    dateIndex = dateIndex.AddDays(1);
                }
            }
            return dictionary;
        }
    }
}
