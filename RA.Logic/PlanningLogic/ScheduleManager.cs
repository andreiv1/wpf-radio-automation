using Microsoft.EntityFrameworkCore;
using RA.Database;
using RA.Dto;
using RA.Logic.PlanningLogic.Interfaces;
using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.PlanningLogic
{
    public class ScheduleManager : IScheduleManager
    {
        public DefaultScheduleDto GetDefaultSchedule(DateTime date)
        {
            var result = GetDefaultScheduleOverviewAsync(date, date).Result.FirstOrDefault();
            if(result.Value == null)
            {
                throw new Exception($"There is no schedule for day {date}.");
            }
            return result.Value;
        }

        public async Task<Dictionary<DateTime, DefaultScheduleDto>> GetDefaultScheduleOverviewAsync(DateTime searchDateStart, DateTime searchDateEnd)
        {
            var dictionary = new Dictionary<DateTime, DefaultScheduleDto>();
            using (var db = new AppDbContext())
            {
                var defaultSchedules = await db.DefaultSchedules
                    .Include(ds => ds.Template)
                    .Where(ds =>
                        (ds.StartDate <= searchDateEnd) &&
                        (!ds.EndDate.HasValue || ds.EndDate >= searchDateStart))
                    .OrderBy(ds => ds.StartDate)
                    .OrderBy(ds => ds.EndDate)
                    .ToListAsync();

                if (defaultSchedules.Count == 0)
                {
                    throw new Exception("No schedules found within the specified date range.");
                }

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
                        Console.WriteLine($"{day}, {dateIndex}: No schedule found.");
                    }
                    dateIndex = dateIndex.AddDays(1);
                }
            }
            return dictionary;
        }
    }
}
