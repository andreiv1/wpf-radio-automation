﻿using RA.Database.Models;
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
        Task DeletePlannedSchedule(int id);
        Task<IDictionary<DateTime, SchedulePlannedDTO?>> GetPlannedSchedulesOverviewAsync(DateTime searchDateStart, DateTime searchDateEnd);
        Task<SchedulePlannedDTO> GetSchedule(int id);
        Task<bool> IsAnyOverlap(SchedulePlannedType type, DateTime start, DateTime? end = null, int? excludeScheduleId = null);
        Task UpdatePlannedSchedule(SchedulePlannedDTO schedule);
    }
}
