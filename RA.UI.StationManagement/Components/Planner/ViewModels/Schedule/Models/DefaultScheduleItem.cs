using CommunityToolkit.Mvvm.ComponentModel;
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
        public int Id { get; set; }

        [ObservableProperty]
        private int templateId;

        [ObservableProperty]
        private string templateName = "";

        [ObservableProperty]
        private DayOfWeek day;

    }
}
