using Microsoft.EntityFrameworkCore;
using RA.Database;
using RA.Database.Models;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public class SchedulesPlannedService : ISchedulesPlannedService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;

        public SchedulesPlannedService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task AddPlannedSchedule(SchedulePlannedDTO schedule)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            SchedulePlanned entity = SchedulePlannedDTO.ToEntity(schedule);
            if(entity.Type == SchedulePlannedType.OneTime)
            {
                entity.EndDate = null;
            }
            await dbContext.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdatePlannedSchedule(SchedulePlannedDTO schedule)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var existingEntity = await dbContext.SchedulesPlanned.FindAsync(schedule.Id);
            if (existingEntity == null) return;
            schedule.Id = null;
            SchedulePlanned entity = SchedulePlannedDTO.ToEntity(schedule);
            //if (entity.Type == SchedulePlannedType.OneTime)
            //{
            //    entity.EndDate = null;
            //}

            existingEntity.Frequency = entity.Frequency;
            existingEntity.StartDate = entity.StartDate;
            if (entity.Type == SchedulePlannedType.OneTime)
            {
                existingEntity.EndDate = null;
            }
            else
            {
                existingEntity.EndDate = entity.EndDate;
            }
            existingEntity.Type = entity.Type;
            existingEntity.TemplateId = entity.TemplateId;
            existingEntity.Name = entity.Name;
            existingEntity.IsMonday = entity.IsMonday;
            existingEntity.IsTuesday = entity.IsTuesday;
            existingEntity.IsWednesday = entity.IsWednesday;
            existingEntity.IsThursday = entity.IsThursday;
            existingEntity.IsFriday = entity.IsFriday;
            existingEntity.IsSaturday = entity.IsSaturday;
            existingEntity.IsSunday = entity.IsSunday;
            dbContext.Update(existingEntity);
            await dbContext.SaveChangesAsync();
            
           
        }

        public async Task<IDictionary<DateTime, SchedulePlannedDTO?>> GetPlannedSchedulesOverviewAsync(DateTime searchDateStart,
                                                                                                       DateTime searchDateEnd)
        {
            var result = new Dictionary<DateTime, SchedulePlannedDTO?>();
            using var dbContext = dbContextFactory.CreateDbContext();

            var recurrentSchedulesInRange = await dbContext.SchedulesPlanned
                .Where(sp => sp.Type == SchedulePlannedType.Recurrent)
                .Include(sp => sp.Template)
                .Where(item => (item.StartDate <= searchDateEnd) &&
                               (item.EndDate >= searchDateStart))
                .OrderBy(item => item.StartDate)
                .ThenBy(item => item.EndDate)
                .Select(item => SchedulePlannedDTO.FromEntity(item))
                .ToListAsync();

            var oneTimeSchedulesInRange = await dbContext.SchedulesPlanned
                .Where(sp => sp.Type == SchedulePlannedType.OneTime)
                .Include(sp => sp.Template)
                .Where(item => (item.StartDate >= searchDateStart) &&
                               (item.StartDate <= searchDateEnd))
                .OrderBy(item => item.StartDate)
                .Select(item => SchedulePlannedDTO.FromEntity(item))
                .ToListAsync();

           
            foreach(var recurrent in recurrentSchedulesInRange)
            {
                if (recurrent.StartDate.HasValue && recurrent.EndDate.HasValue)
                {
                    List<DateTime> recurrentCalculatedDates;

                    if(recurrent.Frequency == SchedulePlannedFrequency.EveryWeek)
                    {
                        recurrentCalculatedDates = CalculateWeeklyRecurrentDates(recurrent);
                    } else if(recurrent.Frequency == SchedulePlannedFrequency.EveryTwoWeeks)
                    {
                        recurrentCalculatedDates = CalculateBiweeklyRecurrentDates(recurrent);
                    } else
                    {
                        throw new InvalidOperationException();
                    }

                    foreach(var day in recurrentCalculatedDates)
                    {
                        if (day >= searchDateStart && day <= searchDateEnd)
                        {
                            result.Add(day, recurrent);
                        }
                    }

                }
            }

            foreach (var onetime in oneTimeSchedulesInRange)
            {
                if (onetime.StartDate != null)
                {
                    if(result.ContainsKey(onetime.StartDate.Value))
                    {
                        result[onetime.StartDate.Value] = onetime;
                    }
                    else
                    {
                        result.Add(onetime.StartDate.Value, onetime);
                    }

                }
                else
                {
                    Debug.WriteLine($"PlannedSchedule: Id={onetime.Id} does not have start date (one time)");
                }
            }
            return result;
        }
        public async Task DeletePlannedSchedule(int id)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            await dbContext.SchedulesPlanned
                .Where(sp => sp.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<SchedulePlannedDTO> GetSchedule(int id)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var entity = await dbContext.SchedulesPlanned
                .Include(sp => sp.Template)
                .FirstOrDefaultAsync(sp => sp.Id == id);
            return SchedulePlannedDTO.FromEntity(entity);
        }
        private HashSet<DayOfWeek> GetDaysSet(SchedulePlannedDTO recurrent)
        {
            HashSet<DayOfWeek> days = new HashSet<DayOfWeek>();
            if (recurrent.IsMonday.HasValue && recurrent.IsMonday.Value)
            {
                days.Add(DayOfWeek.Monday);
            }
            if (recurrent.IsTuesday.HasValue && recurrent.IsTuesday.Value)
            {
                days.Add(DayOfWeek.Tuesday);
            }
            if (recurrent.IsWednesday.HasValue && recurrent.IsWednesday.Value)
            {
                days.Add(DayOfWeek.Wednesday);
            }
            if (recurrent.IsThursday.HasValue && recurrent.IsThursday.Value)
            {
                days.Add(DayOfWeek.Thursday);
            }
            if (recurrent.IsFriday.HasValue && recurrent.IsFriday.Value)
            {
                days.Add(DayOfWeek.Friday);
            }
            if (recurrent.IsSaturday.HasValue && recurrent.IsSaturday.Value)
            {
                days.Add(DayOfWeek.Saturday);
            }
            if (recurrent.IsSunday.HasValue && recurrent.IsSunday.Value)
            {
                days.Add(DayOfWeek.Sunday);
            }
            return days;
        }
        private List<DateTime> CalculateWeeklyRecurrentDates(SchedulePlannedDTO weeklySchedule)
        {
            HashSet<DayOfWeek> days = GetDaysSet(weeklySchedule);
            List<DateTime> recurrentCalculatedDates = new List<DateTime>();

            DateTime currentDate = weeklySchedule.StartDate.Value.Date;
            while (currentDate <= weeklySchedule.EndDate.Value.Date)
            {
                if (days.Contains(currentDate.DayOfWeek))
                {
                    recurrentCalculatedDates.Add(currentDate);
                }
                currentDate = currentDate.AddDays(1);
            }

            return recurrentCalculatedDates;
        }

        private List<DateTime> CalculateBiweeklyRecurrentDates(SchedulePlannedDTO biweeklySchedule)
        {
            HashSet<DayOfWeek> days = GetDaysSet(biweeklySchedule);
            List<DateTime> recurrentCalculatedDates = new List<DateTime>();

            DateTime currentDate = biweeklySchedule.StartDate.Value.Date;

            int weekNumber = 1;
            int dayCount = 0;

            while (currentDate <= biweeklySchedule.EndDate.Value.Date)
            {
                if (days.Contains(currentDate.DayOfWeek) && weekNumber == 1)
                {
                    recurrentCalculatedDates.Add(currentDate);
                }

                currentDate = currentDate.AddDays(1);
                dayCount++;

                // Every 7 days, toggle the week number
                if (dayCount % 7 == 0)
                {
                    weekNumber = weekNumber == 1 ? 2 : 1;
                }
            }


            return recurrentCalculatedDates;
        }

        public async Task<bool> IsAnyOverlap(SchedulePlannedType type, DateTime start, DateTime? end = null, int? excludeScheduleId = null)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            start = start.Date;
            if (end.HasValue) {
                end = end.Value.Date;
            }
            var overlappingScheduleQuery = dbContext.SchedulesPlanned
                .Where(s => s.Type == type);
            if(excludeScheduleId.HasValue)
                overlappingScheduleQuery = overlappingScheduleQuery.Where(s => s.Id != excludeScheduleId);
            if (type == SchedulePlannedType.Recurrent)
            {
                if (end == null) throw new ArgumentNullException($"Recurrent schedule must have an end date.");
               var overlappingSchedule = await overlappingScheduleQuery
                  .FirstOrDefaultAsync(s =>
                      (start <= s.EndDate && end >= s.StartDate)
                      || (start >= s.StartDate && start <= s.EndDate)
                      || (end >= s.StartDate && end <= s.EndDate));

                return overlappingSchedule != null;
            } else
            {
                var overlappingSchedule = await overlappingScheduleQuery
                    .FirstOrDefaultAsync(s => s.StartDate == start);
                return overlappingSchedule != null;
            }
        }
    }
}
