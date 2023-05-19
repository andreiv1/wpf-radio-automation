using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using RA.DAL;
using RA.Database;
using RA.DTO;
using RA.Logic;
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

        public ObservableCollection<ClockDTO> Clocks { get; set; } = new();

        public ObservableCollection<ClockItemModel> ClockItemsForSelectedClock { get; set; } = new();

        public ObservableCollection<ClockPieChartModel> ClockItemsPieChart { get; set; } = new();

        [ObservableProperty]
        private ClockDTO? selectedClock = null;
        partial void OnSelectedClockChanged(ClockDTO? value)
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
                        clockItemModel.Duration = categoryAvgDurations[clockItemDto.CategoryId.Value];
                    }
                    else
                    {
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
            if (SelectedClock == null) return;
            windowService.ShowDialog<TrackSelectViewModel>();
        }

        [RelayCommand]
        private void InsertCategoryRuleToSelectedClock()
        {
            if (SelectedClock == null) return;
            var vm = windowService.ShowDialog<PlannerManageClockCategoryRuleViewModel>(SelectedClock.Id);
            if(vm.SelectedCategory != null)
            {
                DebugHelper.WriteLine(this, $"To add clock rule - {vm.SelectedCategory.Id}");
                int latestIndex = ClockItemsForSelectedClock.Count;
                clocksService.AddClockItem(new ClockItemDTO()
                {
                    OrderIndex = latestIndex,
                    CategoryId = vm.SelectedCategory.Id,
                    ClockId = SelectedClock.Id,
                });

                //Reload clocks from db
                _ = LoadClockItemsForSelectedClock();
            }
        }

        [RelayCommand]
        private void InsertEventRuleToSelectedClock()
        {
            if (SelectedClock == null) return;
            var vm = windowService.ShowDialog<PlannerManageClockEventRuleViewModel>();
            //TODO
        }
        [RelayCommand]
        private void OpenAddClockDialog()
        {
            var result = windowService.ShowDialog<PlannerManageClockViewModel>();
            var dto = ClockModel.ToDto(result.ManagedClock);
            clocksService.AddClock(dto);
            _ = LoadClocks();
        }

        [RelayCommand]
        private void EditClockDialog()
        {
            if (SelectedClock == null) return;
            var result = windowService.ShowDialog<PlannerManageClockViewModel>(SelectedClock.Id ?? throw new ArgumentException("Id cannot be empty."));
            var dto = ClockModel.ToDto(result.ManagedClock);
            clocksService.UpdateClock(dto);
            _ = LoadClocks();

        }

        [RelayCommand]
        private void DuplicateClockDialog()
        {
            if (SelectedClock == null) return;
            windowService.ShowDialog<PlannerManageClockViewModel>(SelectedClock.Id ?? throw new ArgumentException("Id cannot be empty."),
               true);
        }

        [RelayCommand]
        private void RefreshClocks()
        {
            _ = LoadClocks();
        }
        #endregion


    }
}
