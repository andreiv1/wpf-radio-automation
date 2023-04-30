using CommunityToolkit.Mvvm.Input;
using RA.UI.Core.Services.Interfaces;
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
        private readonly IWindowService windowService;

        public ObservableCollection<ScheduleCalendarItem> CalendarItems { get; set; } = new()
        {
            new ScheduleCalendarItem(DateTime.Today.Date, "Test", -1),
            new ScheduleCalendarItem(DateTime.Today.Date.AddDays(1), "Test 2", -1),
        };

        public PlannerScheduleCalendarViewModel(IWindowService windowService)
        {
            this.windowService = windowService;
        }



        #region Commands
        [RelayCommand]
        private void AddScheduleItem()
        {
            windowService.ShowDialog<PlannerManageScheduleItemViewModel>();
        }
        #endregion
    }
}
