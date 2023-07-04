using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public interface ISchedulesPlannedService
    {
        Task AddPlannedSchedule(SchedulePlannedDTO schedule);
        Task<IDictionary<DateTime, SchedulePlannedDTO?>> GetPlannedSchedulesOverviewAsync(DateTime searchDateStart, DateTime searchDateEnd);
    }
}
