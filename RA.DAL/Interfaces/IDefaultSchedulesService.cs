﻿using RA.DTO;

namespace RA.DAL
{
    public interface IDefaultSchedulesService
    {
        IDictionary<DateTime, ScheduleDefaultItemDto?> GetDefaultSchedulesOverview(DateTime searchDateStart, DateTime searchDateEnd);
        Task<IDictionary<DateTime, ScheduleDefaultItemDto?>> GetDefaultSchedulesOverviewAsync(DateTime searchDateStart, DateTime searchDateEnd);
        IEnumerable<ScheduleDefaultDto> GetDefaultSchedules(int skip = 0, int limit = 100, bool ascending = false);

        Task<IDictionary<DayOfWeek, ScheduleDefaultItemDto?>> GetDefaultScheduleItems(ScheduleDefaultDto parentDefaultScheduleDto);
        Task<int> UpdateDefaultScheduleItems(List<ScheduleDefaultItemDto> defaultScheduleItems);
        Task<int> AddDefaultSchedule(ScheduleDefaultDto scheduleDefaultDto, List<ScheduleDefaultItemDto> items);
    }
}