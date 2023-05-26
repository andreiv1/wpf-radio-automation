using CommunityToolkit.Mvvm.ComponentModel;
using RA.DTO;
using RA.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Schedule.Models
{
    public partial class DefaultScheduleItem : ObservableObject
    {
        [ObservableProperty]
        private int? id;

        [ObservableProperty]
        private int templateId;

        [ObservableProperty]
        private string templateName = "";

        [ObservableProperty]
        private DayOfWeek day;

        public bool IsUpdated { get; set; } = false;

        public static ScheduleDefaultItemDTO ToDto(DefaultScheduleItem model, ScheduleDefaultDTO scheduleDto)
        {
            return new ScheduleDefaultItemDTO(scheduleDto)
            {
                Id = model.Id,
                DayOfWeek = model.Day,
                Template = new TemplateDTO(model.TemplateId),
            };
        }
    }
}
