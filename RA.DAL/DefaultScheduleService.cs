using Microsoft.EntityFrameworkCore;
using RA.DAL.Models;
using RA.Database;
using RA.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public class DefaultScheduleService : IDefaultScheduleService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;

        public DefaultScheduleService(IDbContextFactory<AppDbContext> dbContextFactory)
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
                        (ds.EndDate >= searchDateStart))
                    .OrderBy(ds => ds.StartDate)
                    .OrderBy(ds => ds.EndDate)
                    .ToListAsync();

                DateTime dateIndex = searchDateStart;
                while (dateIndex <= searchDateEnd)
                {
                    DayOfWeek day = dateIndex.DayOfWeek;
                    var item = defaultSchedules.Where(ds => ds.DayOfWeek == day &&
                        (ds.StartDate <= searchDateEnd) &&
                        (ds.EndDate >= dateIndex))
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
        public async Task<IEnumerable<DateTimeRange>> GetDefaultSchedulesRangeAsync(int skip = 0, int limit = 100, bool ascending = false)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var query = dbContext.DefaultSchedules
                .Select(s => new { s.StartDate, s.EndDate })
                .Distinct();

            query = ascending
                ? query.OrderBy(s => s.StartDate).ThenBy(s => s.EndDate)
                : query.OrderByDescending(s => s.StartDate).ThenBy(s => s.EndDate);

            var result = await query
                .Select(s => new DateTimeRange(s.StartDate, s.EndDate))
                .Skip(skip)
                .Take(limit)
                .ToListAsync();

            return result;
        }

        public IEnumerable<DateTimeRange> GetDefaultSchedulesRange(int skip = 0, int limit = 100, bool ascending = false)
        {
            return GetDefaultSchedulesRangeAsync(skip, limit, ascending).Result;
        }

        public async Task<IDictionary<DayOfWeek, DefaultScheduleDto?>> GetDefaultScheduleWithTemplate(DateTimeRange range)
        {
            var dictionary = new Dictionary<DayOfWeek, DefaultScheduleDto?>();
            using var dbContext = dbContextFactory.CreateDbContext();
            var data = await Task.Run(() => dbContext.DefaultSchedules
                                .Include(ds => ds.Template)
                                .Where(ds => ds.StartDate.Equals(range.StartDate) &&
                                        ds.EndDate.Equals(range.EndDate))
                                .Select(ds => DefaultScheduleDto.FromEntity(ds))
                                .ToListAsync());
            for (int i = 0; i < 7; i++)
            {
                DefaultScheduleDto? scheduleByDay = data.Where(s => s.Day == (DayOfWeek)i)
                    .FirstOrDefault();

                dictionary.Add((DayOfWeek)i, scheduleByDay);
            }
            return dictionary;
        }
    }
}
