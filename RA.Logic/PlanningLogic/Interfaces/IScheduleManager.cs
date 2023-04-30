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
        Task<Dictionary<DateTime, DefaultScheduleDto>> GetDefaultScheduleOverviewAsync(DateTime searchDateStart, DateTime searchDateEnd);
        DefaultScheduleDto GetDefaultSchedule(DateTime date);

    }
}
