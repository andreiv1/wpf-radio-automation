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

        public IScheduleDTO GetScheduleByDate(DateTime date)
        {
            var defaultSchedule = schedulesDefaultService.GetDefaultSchedulesOverview(date, date);
            IScheduleDTO? result = defaultSchedule.FirstOrDefault().Value;
            if(result == null)
            {
                throw new Exception($"No schedule found (default / planned) for the given date: {date.ToString("dd/MM/yyyy")}.");
            }
            return result;
        }
    }
}
