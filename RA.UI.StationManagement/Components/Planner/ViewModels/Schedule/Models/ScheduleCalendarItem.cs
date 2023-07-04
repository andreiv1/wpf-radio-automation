﻿using CommunityToolkit.Mvvm.ComponentModel;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Schedule.Models
{
    public partial class ScheduleCalendarItem : ObservableObject
    {
        private int? scheduleId;

        [ObservableProperty]
        private DateTime date;

        public DateTime EndDate { get => Date.AddHours(23).AddMinutes(59); }

        [ObservableProperty]
        private string templateName;

        private int templateId;

        public string RecurrenceRule
        {
            get
            {
                return $"RRULE:FREQ=DAILY;UNTIL={EndDate:yyyyMMddTHHmmssZ}";
            }
        }

        public ScheduleCalendarItem()
        {
        }

        public ScheduleCalendarItem(DateTime date, string templateName, int templateId)
        {
            this.date = date;
            this.templateName = templateName;
            this.templateId = templateId;
        }

        public static ScheduleCalendarItem FromDto(ScheduleDefaultItemDTO dto, DateTime date)
        {
            return new ScheduleCalendarItem(date, dto.Template?.Name, 
                dto.Template?.Id ?? -1);
        }

        public static ScheduleCalendarItem FromDto(SchedulePlannedDTO dto, DateTime date)
        {
            return new ScheduleCalendarItem(date, dto.Template?.Name, dto.Template?.Id ?? -1);
        }

    }
}
