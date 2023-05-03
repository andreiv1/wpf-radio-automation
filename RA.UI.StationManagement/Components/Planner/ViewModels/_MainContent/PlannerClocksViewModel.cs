using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using RA.DAL;
using RA.Database;
using RA.DTO;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels.Clocks;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent.Models;
using RA.UI.StationManagement.Dialogs.TrackSelectDialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.MainContent
{
    public partial class PlannerClocksViewModel : ViewModelBase
    {
        private readonly IClocksService clocksService;
        private readonly IWindowService windowService;
        private readonly IDispatcherService dispatcherService;

        public ObservableCollection<ClockDto> Clocks { get; set; } = new();

        public ObservableCollection<ClockItemModel> ClockItemsForSelectedClock { get; set; } = new();

        public ObservableCollection<ClockPieChartModel> ClockItemsPieChart { get; set; } = new();

        [ObservableProperty]
        private ClockDto? selectedClock = null;
        partial void OnSelectedClockChanged(ClockDto? value)
        {
            _ = LoadClockItemsForSelectedClock();
        }
        [ObservableProperty]
        private TimeSpan totalDuration;

        #region Constructor
        public PlannerClocksViewModel(IWindowService windowService, IDispatcherService dispatcherService,
            IClocksService clocksService)
        {
            this.windowService = windowService;
            this.dispatcherService = dispatcherService;
            this.clocksService = clocksService;
            _ = LoadClocks();
            ClockItemsForSelectedClock.CollectionChanged += ClockItemsForSelectedClock_CollectionChanged;
        }
        #endregion
        private void ClockItemsForSelectedClock_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CalculateStartTime();
            FillPieChart();
        }

        #region Data fetching
        private async Task LoadClocks()
        {
            var clocks = await Task.Run(() => clocksService.GetClocksAsync());
            Clocks.Clear();
            foreach (var clock in clocks)
            {
                dispatcherService.InvokeOnUIThread(() =>
                {
                    Clocks.Add(clock);
                });
            }

        }
        private async Task LoadClockItemsForSelectedClock()
        {
            if (SelectedClock != null)
            {
                var clockItems = await Task.Run(() => clocksService.GetClockItemsAsync(SelectedClock.Id.GetValueOrDefault()));
                ClockItemsForSelectedClock.Clear();

                var categoryAvgDurations = await Task.Run(() => clocksService.CalculateAverageDurationsForCategoriesInClockWithId(SelectedClock.Id.GetValueOrDefault()));

                foreach (var clockItemDto in clockItems)
                {
                    ClockItemModel clockItemModel = ClockItemModel.FromDto(clockItemDto);

                    //TODO: Here treat multiple Clock Items type to display String clockItemDto details
                    if (clockItemDto.CategoryId.HasValue)
                    {
                        clockItemModel.ItemDetails = $"From category: {clockItemDto.CategoryName}";
                        clockItemModel.Duration = categoryAvgDurations[clockItemDto.CategoryId.Value];
                    }
                    else
                    {
                        clockItemModel.ItemDetails = "Unknown - TODO";
                        clockItemModel.Duration = TimeSpan.Zero;
                    }
                    dispatcherService.InvokeOnUIThread(() =>
                    {
                        ClockItemsForSelectedClock.Add(clockItemModel);
                    });
                }
            }
        }

        #endregion

        private void CalculateStartTime()
        {
            TotalDuration = TimeSpan.Zero;
            foreach (var item in ClockItemsForSelectedClock)
            {
                item.StartTime = TotalDuration;
                TotalDuration += item.Duration;
            }
        }
        private void FillPieChart()
        {
            ClockItemsPieChart.Clear();
            int totalSeconds = 0;
            foreach (var clock in ClockItemsForSelectedClock)
            {
                ClockItemsPieChart.Add(new ClockPieChartModel(clock.CategoryName, (int)clock.Duration.TotalSeconds));
                totalSeconds += (int)clock.Duration.TotalSeconds;
            }
            if (totalSeconds < 3600)
            {
                ClockItemsPieChart.Add(new ClockPieChartModel("Unfilled clock time", 3600 - totalSeconds));
            }
        }

        #region Commands
        [RelayCommand]
        private void InsertTrackToSelectedClock()
        {
            windowService.ShowDialog<TrackSelectViewModel>();
        }

        [RelayCommand]
        private void InsertCategoryRuleToSelectedClock()
        {
            windowService.ShowDialog<PlannerManageClockCategoryRuleViewModel>(5);
        }

        [RelayCommand]
        private void OpenAddClockDialog()
        {
            windowService.ShowDialog<PlannerManageClockViewModel>();
        }

        [RelayCommand]
        private void EditClockDialog()
        {
            if (SelectedClock == null) return;
            windowService.ShowDialog<PlannerManageClockViewModel>(SelectedClock.Id ?? throw new ArgumentException("Id cannot be empty."));
        }

        [RelayCommand]
        private void DuplicateClockDialog()
        {
            if (SelectedClock == null) return;
            windowService.ShowDialog<PlannerManageClockViewModel>(SelectedClock.Id ?? throw new ArgumentException("Id cannot be empty."),
               true);
        }
        #endregion


    }
}
