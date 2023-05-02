using RA.DTO;

namespace RA.DAL
{
    public interface IDefaultSchedulesService
    {
        IDictionary<DateTime, ScheduleDefaultItemDto?> GetDefaultScheduleOverview(DateTime searchDateStart, DateTime searchDateEnd);
        Task<IDictionary<DateTime, ScheduleDefaultItemDto?>> GetDefaultScheduleOverviewAsync(DateTime searchDateStart, DateTime searchDateEnd);
        IEnumerable<ScheduleDefaultDto> GetDefaultSchedules(int skip = 0, int limit = 100, bool ascending = false);
        

    }
}