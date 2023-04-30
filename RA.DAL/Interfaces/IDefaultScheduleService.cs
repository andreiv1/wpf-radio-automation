using RA.DAL.Models;
using RA.DTO;

namespace RA.DAL
{
    public interface IDefaultScheduleService
    {
        Task<IDictionary<DateTime, DefaultScheduleDto>> GetDefaultScheduleOverviewAsync(DateTime searchDateStart, DateTime searchDateEnd);

        IDictionary<DateTime, DefaultScheduleDto> GetDefaultScheduleOverview(DateTime searchDateStart, DateTime searchDateEnd);
        Task<IEnumerable<DateTimeRange>> GetDefaultSchedulesRangeAsync(int skip = 0, int limit = 100, bool ascending = false);
        IEnumerable<DateTimeRange> GetDefaultSchedulesRange(int skip = 0, int limit = 100, bool ascending = false);
        Task<IDictionary<DayOfWeek, DefaultScheduleDto?>> GetDefaultScheduleWithTemplateAsync(DateTimeRange range);
    }
}