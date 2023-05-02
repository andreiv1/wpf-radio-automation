using Microsoft.EntityFrameworkCore;
using RA.DAL.Models;
using RA.Database;
using RA.Database.Models;
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
            throw new NotImplementedException();
            //using (var dbContext = dbContextFactory.CreateDbContext())
            //{
            //    var defaultSchedules = await dbContext.DefaultSchedules
            //        .Include(ds => ds.Template)
            //        .Where(ds =>
            //            (ds.StartDate <= searchDateEnd) &&
            //            (ds.EndDate >= searchDateStart))
            //        .OrderBy(ds => ds.StartDate)
            //        .OrderBy(ds => ds.EndDate)
            //        .ToListAsync();

            //    DateTime dateIndex = searchDateStart;
            //    while (dateIndex <= searchDateEnd)
            //    {
            //        DayOfWeek day = dateIndex.DayOfWeek;
            //        var item = defaultSchedules.Where(ds => ds.DayOfWeek == day &&
            //            (ds.StartDate <= searchDateEnd) &&
            //            (ds.EndDate >= dateIndex))
            //            .FirstOrDefault();

            //        if (item != null)
            //        {
            //            dictionary[dateIndex] = DefaultScheduleDto.FromEntity(item);
            //        }
            //        else
            //        {
            //            //There is no schedule found for this day, so we send an empty schedule
            //            dictionary[dateIndex] = new DefaultScheduleDto();
            //        }
            //        dateIndex = dateIndex.AddDays(1);
            //    }
            //}
            return dictionary;
        }
        public async Task<IEnumerable<DateTimeRange>> GetDefaultSchedulesRangeAsync(int skip = 0, int limit = 100, bool ascending = false)
        {
            throw new NotImplementedException();
            //using var dbContext = dbContextFactory.CreateDbContext();
            //var query = dbContext.DefaultSchedules
            //    .Select(s => new { s.StartDate, s.EndDate })
            //    .Distinct();

            //query = ascending
            //    ? query.OrderBy(s => s.StartDate).ThenBy(s => s.EndDate)
            //    : query.OrderByDescending(s => s.StartDate).ThenBy(s => s.EndDate);

            //var result = await query
            //    .Select(s => new DateTimeRange(s.StartDate, s.EndDate))
            //    .Skip(skip)
            //    .Take(limit)
            //    .ToListAsync();

            //return result;
        }
        public IEnumerable<DateTimeRange> GetDefaultSchedulesRange(int skip = 0, int limit = 100, bool ascending = false)
        {
            return GetDefaultSchedulesRangeAsync(skip, limit, ascending).Result;
        }
        public async Task<IDictionary<DayOfWeek, DefaultScheduleDto?>> GetDefaultScheduleWithTemplateAsync(DateTimeRange range)
        {
            throw new NotImplementedException();
            //var dictionary = new Dictionary<DayOfWeek, DefaultScheduleDto?>();
            //using var dbContext = dbContextFactory.CreateDbContext();
            //var data = await Task.Run(() => dbContext.DefaultSchedules
            //                    .Include(ds => ds.Template)
            //                    .Where(ds => ds.StartDate.Equals(range.StartDate) &&
            //                            ds.EndDate.Equals(range.EndDate))
            //                    .Select(ds => DefaultScheduleDto.FromEntity(ds))
            //                    .ToListAsync());
            //for (int i = 0; i < 7; i++)
            //{
            //    DefaultScheduleDto? scheduleByDay = data.Where(s => s.Day == (DayOfWeek)i)
            //        .FirstOrDefault();

            //    dictionary.Add((DayOfWeek)i, scheduleByDay);
            //}
            //return dictionary;
        }

        public async Task<bool> AddDefaultScheduleItemsAsync(List<DefaultScheduleDto> defaultScheduleItems, DateTimeRange scheduleRange)
        {
            //if (defaultScheduleItems == null || defaultScheduleItems?.Count == 0)
            //{
            //    throw new ArgumentNullException(nameof(defaultScheduleItems), "The default schedule items list cannot be null or empty.");
            //}

            //var allDays = new HashSet<DayOfWeek>(Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>());
            //var daysInList = new HashSet<DayOfWeek>(defaultScheduleItems!.Select(d => d.Day));

            //if (!allDays.IsSubsetOf(daysInList))
            //{
            //    throw new ArgumentException("The default schedule items list does not contain all the possible DayOfWeek values.", nameof(defaultScheduleItems));
            //}

            //foreach(var item in defaultScheduleItems)
            //{
            //    if(item.TemplateDto?.Id == 0 || item.TemplateDto == null)
            //    {
            //        throw new ArgumentException("A default schedule must have a template set.");
            //    }
            //}

            //var hasAllTemplatesSet = defaultScheduleItems.Select(x => x.TemplateDto)
            //    .Where(x => x?.Id != 0).Count() > 0;

            //foreach(var scheduleDto in defaultScheduleItems)
            //{
            //    scheduleDto.StartDate = scheduleRange.StartDate;
            //    scheduleDto.EndDate = scheduleRange.EndDate;
            //}
            //var data = defaultScheduleItems.Select(dto => DefaultScheduleDto.ToEntity(dto)).ToList();
            //using var dbContext = dbContextFactory.CreateDbContext();

            //await dbContext.DefaultSchedules
            //    .AddRangeAsync(data.Where(x => x.Id == 0));

            //dbContext.DefaultSchedules
            //   .UpdateRange(data.Where(x => x.Id != 0));
            //await dbContext.SaveChangesAsync();
            throw new NotImplementedException();
            return true;

        }
    }
}
