using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels.Schedule.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Schedule
{
    public partial class PlannerScheduleCalendarViewModel : ViewModelBase
    {
        public ObservableCollection<ScheduleCalendarItem> CalendarItems { get; set; } = new()
        {
            new ScheduleCalendarItem(DateTime.Today.Date, "Test", -1),
            new ScheduleCalendarItem(DateTime.Today.Date.AddDays(1), "Test 2", -1),
        };
    }
}
