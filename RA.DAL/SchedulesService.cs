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

        public IScheduleDTO? GetScheduleByDate(DateTime date)
        {
            var defaultSchedule = schedulesDefaultService.GetDefaultSchedulesOverview(date, date);
            IScheduleDTO? result = defaultSchedule.FirstOrDefault().Value;
            return result;
        }

        public async Task<IDictionary<DateTime, IScheduleDTO?>> GetSchedulesOverview(DateTime searchDateStart, DateTime searchDateEnd)
        {
            Dictionary<DateTime, IScheduleDTO?> result = new();
            

            return result;
        }
    }
}
