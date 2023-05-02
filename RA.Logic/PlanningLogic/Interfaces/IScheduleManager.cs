using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.PlanningLogic.Interfaces
{
    public interface IScheduleManager
    {
        Task<Dictionary<DateTime, ScheduleDefaultDto>> GetDefaultScheduleOverviewAsync(DateTime searchDateStart, DateTime searchDateEnd);
        ScheduleDefaultDto GetDefaultSchedule(DateTime date);

    }
}
