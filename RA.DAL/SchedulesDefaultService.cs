using Microsoft.EntityFrameworkCore;
using RA.Database;
using RA.Database.Models;
using RA.DTO;
using System.Reflection.Metadata.Ecma335;

namespace RA.DAL
{
    public class SchedulesDefaultService : ISchedulesDefaultService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;

        public SchedulesDefaultService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<IDictionary<DateTime, ScheduleDefaultItemDTO?>> GetDefaultSchedulesOverviewAsync(DateTime searchDateStart, 
            DateTime searchDateEnd)
        {
            var dictionary = new Dictionary<DateTime, ScheduleDefaultItemDTO?>();
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
                .Select(item => ScheduleDefaultDTO.FromEntity(item))
                .ToListAsync();

            DateTime dateIndex = searchDateStart;
            while (dateIndex <= searchDateEnd)
            {
                DayOfWeek day = dateIndex.DayOfWeek;
                var schedule = defaultSchedulesInRange.Where(schedule => (schedule.StartDate <= searchDateEnd)
                                                                        && (dateIndex <= schedule.EndDate) 
                                                                        && dateIndex >= schedule.StartDate)
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

        public IDictionary<DateTime, ScheduleDefaultItemDTO?> GetDefaultSchedulesOverview(DateTime searchDateStart, DateTime searchDateEnd)
        {
            return GetDefaultSchedulesOverviewAsync(searchDateStart, searchDateEnd).Result;
        }
        public IEnumerable<ScheduleDefaultDTO> GetDefaultSchedules(int skip = 0, int limit = 100, bool ascending = false)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var schedules = dbContext.SchedulesDefault
                .Skip(skip)
                .Take(limit)
                .AsEnumerable();
            schedules = ascending ?
                schedules.OrderBy(s => s.StartDate).ThenBy(s => s.EndDate) :
                schedules.OrderByDescending(s => s.StartDate).ThenBy(s => s.EndDate);
            foreach(var schedule in schedules)
            {
                yield return ScheduleDefaultDTO.FromEntity(schedule);
            }
        }

        public async Task<IDictionary<DayOfWeek, ScheduleDefaultItemDTO?>> GetDefaultScheduleItems(ScheduleDefaultDTO parentDefaultScheduleDto)
        {
            SortedDictionary<DayOfWeek, ScheduleDefaultItemDTO?> result = new();
            using var dbContext = dbContextFactory.CreateDbContext();
            var items = await dbContext.ScheduleDefaultItems
                .Include(s => s.Template)
                .Where(s => s.ScheduleId == parentDefaultScheduleDto.Id)
                .OrderBy(s => s.DayOfWeek)
                .Select(s => ScheduleDefaultItemDTO.FromEntity(s, parentDefaultScheduleDto))
                .ToListAsync();

            for(DayOfWeek day = DayOfWeek.Sunday; day <= DayOfWeek.Saturday; day++)
            {
                result.Add(day, items.Where(itm => itm.DayOfWeek == day).FirstOrDefault());
            }
            return result;
        }

        public async Task<int> UpdateDefaultScheduleItems(List<ScheduleDefaultItemDTO> defaultScheduleItems)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var toUpdate = defaultScheduleItems
                .Select(s => ScheduleDefaultItemDTO.ToEntity(s))
                .ToList();

            foreach(var item in toUpdate)
            {
                item.Template = dbContext.AttachOrGetTrackedEntity<Template>(item.Template);
            }
            dbContext.UpdateRange(toUpdate);
            await dbContext.SaveChangesAsync();
            return toUpdate.Count;
        }

        public async Task<int> AddDefaultSchedule(ScheduleDefaultDTO scheduleDefaultDto)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var schedule = ScheduleDefaultDTO.ToEntity(scheduleDefaultDto);
            schedule.ScheduleDefaultItems = scheduleDefaultDto?.Items?.Select(itm => ScheduleDefaultItemDTO.ToEntity(itm)).ToList();
            if(schedule.ScheduleDefaultItems == null)
            {
                throw new ArgumentNullException($"The schedule can't be added without items (ScheduleDefaultItems).");
            }
            foreach(var itm in schedule.ScheduleDefaultItems)
            {
                itm.Schedule = schedule;
                itm.ScheduleId = 0;
                itm.Template = dbContext.AttachOrGetTrackedEntity(itm.Template);
            }
            await dbContext.AddAsync(schedule);
            return await dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsAnyOverlap(DateTime start, DateTime end)
        {
            start = start.Date;
            end = end.Date;
            using var dbContext = dbContextFactory.CreateDbContext();
            var overlappingSchedule = await dbContext.SchedulesDefault
               .FirstOrDefaultAsync(s =>
                   (start <= s.EndDate && end >= s.StartDate)
                   || (start >= s.StartDate && start <= s.EndDate)
                   || (end >= s.StartDate && end <= s.EndDate));


            return overlappingSchedule != null;

       }
    }
}
