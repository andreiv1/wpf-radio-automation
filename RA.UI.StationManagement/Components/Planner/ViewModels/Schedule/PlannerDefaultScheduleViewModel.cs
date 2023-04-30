using CommunityToolkit.Mvvm.ComponentModel;
using RA.DAL;
using RA.DAL.Models;
using RA.UI.Core.Services;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Schedule
{
    public partial class PlannerDefaultScheduleViewModel : ViewModelBase
    {
        private readonly IDispatcherService dispatcherService;
        private readonly IDefaultScheduleService defaultScheduleService;
        public ObservableCollection<DateTimeRange> DefaultIntervals { get; private set; } = new();

        [ObservableProperty]
        private DateTimeRange? selectedInterval;

        public PlannerDefaultScheduleViewModel(IDispatcherService dispatcherService,
            IDefaultScheduleService defaultScheduleService)
        {
            this.dispatcherService = dispatcherService;
            this.defaultScheduleService = defaultScheduleService;
            _ = LoadIntervals();
        }

        private async Task LoadIntervals()
        {
            var intervals = await Task.Run(() => defaultScheduleService.GetDefaultSchedulesRangeAsync(0, 500));
            DefaultIntervals.Clear();
            foreach(var interval in intervals)
            {
                dispatcherService.InvokeOnUIThread(() =>
                {
                    DefaultIntervals.Add(interval);
                });
                
            }
        }
    }
}
