using RA.DTO;

namespace RA.DAL
{
    public interface ISchedulesDefaultService
    {
        IDictionary<DateTime, ScheduleDefaultItemDTO?> GetDefaultSchedulesOverview(DateTime searchDateStart, DateTime searchDateEnd);
        Task<IDictionary<DateTime, ScheduleDefaultItemDTO?>> GetDefaultSchedulesOverviewAsync(DateTime searchDateStart, DateTime searchDateEnd);
        IEnumerable<ScheduleDefaultDTO> GetDefaultSchedules(int skip = 0, int limit = 100, bool ascending = false);

        Task<IDictionary<DayOfWeek, ScheduleDefaultItemDTO?>> GetDefaultScheduleItems(ScheduleDefaultDTO parentDefaultScheduleDto);
        Task<int> UpdateDefaultScheduleItems(List<ScheduleDefaultItemDTO> defaultScheduleItems);
        Task<int> AddDefaultSchedule(ScheduleDefaultDTO scheduleDefaultDto);
        Task<bool> IsAnyOverlap(DateTime start, DateTime end);
    }
}