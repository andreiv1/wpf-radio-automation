using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DAL.Models;
using RA.DTO;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels.Schedule.Models;
using Syncfusion.UI.Xaml.Scheduler;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Schedule
{
    public partial class PlannerScheduleCalendarViewModel : ViewModelBase
    {
        private readonly IWindowService windowService;
        private readonly ISchedulesService schedulesService;

        public ObservableCollection<ScheduleCalendarItem> CalendarItems { get; private set; } = new()
        {
            new ScheduleCalendarItem(new DateTime(2010, 1, 1), "Loading", -1)
        };

        [ObservableProperty]
        private bool isLoadingCalendar;

        public PlannerScheduleCalendarViewModel(IWindowService windowService,
                                                ISchedulesService schedulesService)
        {
            this.windowService = windowService;
            this.schedulesService = schedulesService;

            _ = InitSchedule();
        }

        private async Task InitSchedule()
        {
            IsLoadingCalendar = true;
            var monthDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var initCalendar = await schedulesService
                .GetSchedulesOverview(monthDate - new TimeSpan(10, 0, 0, 0), monthDate.AddDays(40));
            foreach(var item in initCalendar)
            {
                if (item.Value != null && item.Value.Template != null)
                {
                    if(item.Value is ScheduleDefaultItemDTO defaultSchedule){
                        CalendarItems.Add(ScheduleCalendarItem.FromDto(defaultSchedule, item.Key));
                    } else if(item.Value is SchedulePlannedDTO plannedSchedule)
                    {
                        CalendarItems.Add(ScheduleCalendarItem.FromDto(plannedSchedule, item.Key));
                    }
                }
                    
            }
            IsLoadingCalendar = false;
        }

        private async Task LoadSchedule(DateTimeRange range)
        {
            CalendarItems.Clear();
            IsLoadingCalendar = true;
            var calendar = await schedulesService
                .GetSchedulesOverview(range.StartDate, range.EndDate);

            IsLoadingCalendar = false;
            foreach(var item in calendar)
            {
                if (!string.IsNullOrEmpty(item.Value?.Template?.Name))
                {
                    if (item.Value is ScheduleDefaultItemDTO defaultSchedule)
                    {
                        CalendarItems.Add(ScheduleCalendarItem.FromDto(defaultSchedule, item.Key));
                    }
                    else if (item.Value is SchedulePlannedDTO plannedSchedule)
                    {
                        CalendarItems.Add(ScheduleCalendarItem.FromDto(plannedSchedule, item.Key));
                    }
                }
            }
            IsLoadingCalendar = false;
        }
        //Commands
        [RelayCommand]
        private void LoadScheduleOnDemand(object parameter)
        {
            if (parameter == null) return;

            var eventArgs = parameter as QueryAppointmentsEventArgs;
            _ = LoadSchedule(new DateTimeRange(eventArgs.VisibleDateRange
                .StartDate, eventArgs.VisibleDateRange.EndDate));

        }
        [RelayCommand]
        private void AddScheduleItem()
        {
            var vm = windowService.ShowDialog<PlannerManageScheduleItemViewModel>();
            _ = LoadSchedule(new DateTimeRange (vm.StartDate.AddDays(-60), vm.EndDate.AddDays(60)));
        }

        [RelayCommand]
        private void EditItem()
        {
            //TODO
            throw new NotImplementedException();
        }

        [RelayCommand]
        private void DeleteItem()
        {
            //TODO
            throw new NotImplementedException();
        }

        [RelayCommand]
        private void ViewItem()
        {
            throw new NotImplementedException();
        }

    }
}
