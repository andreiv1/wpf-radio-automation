using CommunityToolkit.Mvvm.ComponentModel;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.MainContent.Models
{
    public partial class TemplateClockItemModel : ObservableObject
    {
        [ObservableProperty]
        private int clockId;

        [ObservableProperty]
        private String clockName;

        [ObservableProperty]
        private int clockSpan;

        [ObservableProperty]
        private DateTime startTime;

        partial void OnStartTimeChanged(DateTime value)
        {
            // Round start time to the nearest hour
            TimeSpan time = value.TimeOfDay;
            DateTime newStartTime = value;

            if (time.Minutes < 30)
            {
                // Round down to the start of the current hour
                newStartTime = newStartTime.AddMinutes(-time.Minutes);
            }
            else
            {
                // Round up to the start of the next hour
                var topTime = new TimeSpan(newStartTime.Hour + 1, 0, 0);
                newStartTime = newStartTime + topTime.Subtract(new TimeSpan(newStartTime.Hour, time.Minutes, 0));
            }
            StartTime = newStartTime;
        }

        [ObservableProperty]
        private DateTime endTime;

        partial void OnEndTimeChanged(DateTime value)
        {
            //Allow end only at :00
            TimeSpan time = value.TimeOfDay;
            DateTime newEndTime = value;

            if (time.Minutes < 30)
            {
                //Round to the start hour
                newEndTime = newEndTime.AddMinutes(-time.Minutes);
            }
            else
            {
                //Round to the next hour
                var topTime = new TimeSpan(newEndTime.Hour + 1, 0, 0);
                newEndTime = newEndTime + topTime.Subtract(new TimeSpan(newEndTime.Hour, time.Minutes, 0));
            }
            EndTime = newEndTime;
        }

        [ObservableProperty]
        private TemplateDTO template;
        public static TemplateClockItemModel FromDto(TemplateClockDTO dto)
        {
            return new TemplateClockItemModel()
            {
                ClockId = dto.ClockId,
                ClockName = dto.ClockName,
                ClockSpan = dto.ClockSpan,
                StartTime = DateTime.Today.Date.Add(dto.StartTime),
                EndTime = DateTime.Today.Date.Add(dto.StartTime) + TimeSpan.FromHours(dto.ClockSpan)
            };
        }
    }
}
