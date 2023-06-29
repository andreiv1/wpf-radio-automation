using Microsoft.EntityFrameworkCore;
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
    public class SchedulesPlannedService : ISchedulesPlannedService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;

        public SchedulesPlannedService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<int> AddPlannedSchedule(SchedulePlannedDTO schedule)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            SchedulePlanned entity = SchedulePlannedDTO.ToEntity(schedule);
            await dbContext.AddAsync(entity);
            return await dbContext.SaveChangesAsync();
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
                HashSet<DayOfWeek> days = new HashSet<DayOfWeek>();
                if(recurrent.IsMonday.HasValue && recurrent.IsMonday.Value)
                {
                    days.Add(DayOfWeek.Monday);
                } 
                if(recurrent.IsTuesday.HasValue && recurrent.IsTuesday.Value)
                {
                    days.Add(DayOfWeek.Tuesday);
                } 
                if(recurrent.IsWednesday.HasValue && recurrent.IsWednesday.Value)
                {
                    days.Add(DayOfWeek.Wednesday);
                } 
                if(recurrent.IsThursday.HasValue && recurrent.IsThursday.Value)
                {
                    days.Add(DayOfWeek.Thursday);
                }
                if(recurrent.IsFriday.HasValue && recurrent.IsFriday.Value)
                {
                    days.Add(DayOfWeek.Friday);
                } 
                if(recurrent.IsSaturday.HasValue && recurrent.IsSaturday.Value)
                {
                    days.Add(DayOfWeek.Saturday);
                } 
                if(recurrent.IsSunday.HasValue && recurrent.IsSunday.Value)
                {
                    days.Add(DayOfWeek.Sunday);
                }
                List<DateTime> recurrentCalculatedDates = new List<DateTime>();
             
 
                for(DateTime dateIndex = searchDateStart; dateIndex <= searchDateEnd; dateIndex = dateIndex.AddDays(1))
                {
                    if(days.Contains(dateIndex.DayOfWeek))
                    {
                        recurrentCalculatedDates.Add(dateIndex);
                    }
                }
            }
            return result;
        }
    }
}
