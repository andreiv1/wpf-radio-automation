using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DAL.Models;
using RA.Logic;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels.Schedule.Models;
using Syncfusion.UI.Xaml.Scheduler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Schedule
{
    public partial class PlannerScheduleCalendarViewModel : ViewModelBase
    {
        private readonly IWindowService windowService;
        private readonly ISchedulesDefaultService defaultScheduleService;

        public ObservableCollection<ScheduleCalendarItem> CalendarItems { get; private set; } = new()
        {
            new ScheduleCalendarItem(new DateTime(2010, 1, 1), "Loading", -1)
        };
        //{
        //    new ScheduleCalendarItem(DateTime.Today.Date, "Test", -1),
        //    new ScheduleCalendarItem(DateTime.Today.Date.AddDays(1), "Test 2", -1),
        //};

        [ObservableProperty]
        private bool isLoadingCalendar;

        public PlannerScheduleCalendarViewModel(IWindowService windowService, 
            ISchedulesDefaultService defaultScheduleService)
        {
            this.windowService = windowService;
            this.defaultScheduleService = defaultScheduleService;
            _ = InitSchedule();
        }

        private async Task InitSchedule()
        {
            IsLoadingCalendar = true;
            var monthDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var initCalendar = await defaultScheduleService
                .GetDefaultSchedulesOverviewAsync(monthDate - new TimeSpan(10, 0, 0, 0), monthDate.AddDays(40));
            foreach(var item in initCalendar)
            {
                //throw new NotImplementedException();
                if (item.Value != null && item.Value.Template != null)
                    CalendarItems.Add(ScheduleCalendarItem.FromDto(item.Value, item.Key));
            }
            IsLoadingCalendar = false;
        }

        private async Task LoadSchedule(DateTimeRange range)
        {
            CalendarItems.Clear();
            IsLoadingCalendar = true;
            var calendar = await defaultScheduleService.GetDefaultSchedulesOverviewAsync(range.StartDate, range.EndDate);
            IsLoadingCalendar = false;
            foreach(var item in calendar)
            {
                if (!String.IsNullOrEmpty(item.Value?.Template?.Name))
                {
                    CalendarItems.Add(ScheduleCalendarItem.FromDto(item.Value, item.Key));
                }
            }
            IsLoadingCalendar = false;
        }
        #region Commands
        [RelayCommand]
        private void LoadScheduleOnDemand(object parameter)
        {
            if(parameter == null)
            {
                return;
            }

            var eventArgs = parameter as QueryAppointmentsEventArgs;
            _ = LoadSchedule(new DateTimeRange(eventArgs.VisibleDateRange
                .StartDate, eventArgs.VisibleDateRange.EndDate));

        }
        [RelayCommand]
        private void AddScheduleItem()
        {
            windowService.ShowDialog<PlannerManageScheduleItemViewModel>();
        }
        #endregion
    }
}
