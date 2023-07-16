using CommunityToolkit.Mvvm.ComponentModel;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Schedule.Models
{
    public partial class ScheduleCalendarItem : ObservableObject
    {
        private static readonly Dictionary<string, Brush> backgroundDictionary = new()
        {
            {"Default", new SolidColorBrush(Colors.DarkGray) },
            {"PlannedOneTime", new SolidColorBrush(Colors.Red) },
            {"PlannedRecurrent", new SolidColorBrush(Colors.Green) },
            {"None", new SolidColorBrush(Colors.Blue) },
        };
        private int? scheduleDefaultItemId;
        private int? schedulePlannedId;

        private string scheduleType = "";

        [ObservableProperty]
        private DateTime date;
        public DateTime EndDate { get => Date.AddHours(23).AddMinutes(59); }

        [ObservableProperty]
        private string itemDisplay = "";

        private int templateId;

        public Brush Background
        {
            get
            {
                switch (scheduleType)
                {
                    case "default":
                        return backgroundDictionary["Default"];
                    case "onetime":
                        return backgroundDictionary["PlannedOneTime"];
                    case "recurrent":
                        return backgroundDictionary["PlannedRecurrent"];
                    default:
                        return backgroundDictionary["None"];
                }
            }
        }
        public Brush Foreground => new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);

        public string ScheduleType => scheduleType;

        public int SchedulePlannedId => schedulePlannedId ?? -1;

        public ScheduleCalendarItem()
        {
        }

        public ScheduleCalendarItem(DateTime date, string scheduleName, string templateName, int templateId)
        {
            this.date = date;
            //TODO item display format: Schedule name (using template 'Template name') 
            this.itemDisplay = $"{scheduleName}\n(Template: {templateName})";
            this.templateId = templateId;
        }

        public static ScheduleCalendarItem FromDto(ScheduleDefaultItemDTO dto, DateTime date)
        {
            var item = new ScheduleCalendarItem(date, 
                dto.Schedule?.Name ?? "unknown",
                dto.Template?.Name ?? "unknown",
                dto.Template?.Id ?? -1);
            item.scheduleType = "default";
            item.scheduleDefaultItemId = dto.Id;
            return item;
        }

        public static ScheduleCalendarItem FromDto(SchedulePlannedDTO dto, DateTime date)
        {
            var item = new ScheduleCalendarItem(date,
               dto.Name ?? "unknown",
               dto.Template?.Name ?? "unknown",
               dto.Template?.Id ?? -1);
            item.scheduleType = dto.Type == Database.Models.SchedulePlannedType.OneTime ? "onetime" : "recurrent";
            item.schedulePlannedId = dto.Id;
            return item;
        }

    }
}
