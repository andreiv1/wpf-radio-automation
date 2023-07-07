using RA.DAL.Exceptions;
using RA.DTO;
using RA.DTO.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public class SchedulesService : ISchedulesService
    {
        private readonly ISchedulesDefaultService schedulesDefaultService;
        private readonly ISchedulesPlannedService schedulesPlannedService;

        public SchedulesService(ISchedulesDefaultService schedulesDefaultService, ISchedulesPlannedService schedulesPlannedService)
        {
            this.schedulesDefaultService = schedulesDefaultService;
            this.schedulesPlannedService = schedulesPlannedService;
        }

        public async Task<IScheduleDTO?> GetScheduleByDate(DateTime date)
        {
            var defaultSchedule = await this.GetSchedulesOverview(date, date);
            IScheduleDTO? result = defaultSchedule.FirstOrDefault().Value;
            return result;
        }

        public async Task<IDictionary<DateTime, IScheduleDTO?>> GetSchedulesOverview(DateTime searchDateStart, DateTime searchDateEnd)
        {
            Dictionary<DateTime, IScheduleDTO?> result = new();
            var defaultSchedules = await schedulesDefaultService.GetDefaultSchedulesOverviewAsync(searchDateStart, searchDateEnd);
            var plannedSchedules = await schedulesPlannedService.GetPlannedSchedulesOverviewAsync(searchDateStart, searchDateEnd);

            foreach(var itm in defaultSchedules)
            {
                result.Add(itm.Key, itm.Value);
            }

            //Planned schedule override existing default
            foreach (var itm in plannedSchedules)
            {
                if(result.ContainsKey(itm.Key))
                {
                    result[itm.Key] = itm.Value;
                }
                else
                {
                    result.Add(itm.Key, itm.Value);
                }

            }

            return result;
        }
    }
}
