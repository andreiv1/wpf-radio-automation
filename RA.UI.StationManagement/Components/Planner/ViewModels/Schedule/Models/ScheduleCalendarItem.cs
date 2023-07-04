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
        private int? scheduleId;

        private string scheduleType = "";

        [ObservableProperty]
        private DateTime date;
        public DateTime EndDate { get => Date.AddHours(23).AddMinutes(59); }

        [ObservableProperty]
        private string itemDisplay;

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

        public ScheduleCalendarItem()
        {
        }

        public ScheduleCalendarItem(DateTime date, string templateName, int templateId)
        {
            this.date = date;
            this.itemDisplay = templateName;
            this.templateId = templateId;
        }

        public static ScheduleCalendarItem FromDto(ScheduleDefaultItemDTO dto, DateTime date)
        {
            var item = new ScheduleCalendarItem(date, dto.Template?.Name,
                dto.Template?.Id ?? -1);
            item.scheduleType = "default";
            return item;
        }

        public static ScheduleCalendarItem FromDto(SchedulePlannedDTO dto, DateTime date)
        {
            var item = new ScheduleCalendarItem(date, dto.Template?.Name,
               dto.Template?.Id ?? -1);
            item.scheduleType = dto.Type == Database.Models.SchedulePlannedType.OneTime ? "onetime" : "recurrent";
            return item;
        }

    }
}
