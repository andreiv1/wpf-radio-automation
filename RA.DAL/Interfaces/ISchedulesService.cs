using RA.DTO.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public interface ISchedulesService
    {
        Task<IScheduleDTO?> GetScheduleByDate(DateTime date);
        Task<IDictionary<DateTime, IScheduleDTO?>> GetSchedulesOverview(DateTime searchDateStart, DateTime searchDateEnd);
    }
}
