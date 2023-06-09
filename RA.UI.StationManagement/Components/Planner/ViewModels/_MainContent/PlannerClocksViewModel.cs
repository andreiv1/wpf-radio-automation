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

        [ObservableProperty]
        private ClockItemModel? selectedClockItem = null;
        public ObservableCollection<object> SelectedClockItems { get; set; } = new();
        partial void OnSelectedClockChanged(ClockDTO? value)
        {
            _ = LoadClockItemsForSelectedClock();
            SelectedClockItem = null;
            ClockItemsForSelectedClock.Clear();
            SelectedClockItems.Clear();
        }
        [ObservableProperty]
        private TimeSpan totalDuration;

        #region Constructor
        public PlannerClocksViewModel(IWindowService windowService,
                                      IDispatcherService dispatcherService,
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
                var clockItems = await Task.Run(() => clocksService.GetClockItemsAsync(SelectedClock.Id));
                ClockItemsForSelectedClock.Clear();

                var categoryAvgDurations = await Task.Run(() => clocksService.CalculateAverageDurationsForCategoriesInClockWithId(SelectedClock.Id));

                var clockItemsNormal = clockItems.Where(ci => ci.OrderIndex >= 0)
                    .ToList();
                foreach (var clockItemDto in clockItemsNormal)
                {

                    var model = new ClockItemModel(clockItemDto);
                    if(clockItemDto is ClockItemCategoryDTO category 
                        && category.CategoryId.HasValue)
                    {
                        model.Duration = categoryAvgDurations[category.CategoryId.Value];

                        
                    }
                   
                    else if(clockItemDto is ClockItemTrackDTO trackItem)
                    {
                        model.Duration = trackItem.TrackDuration;
                    }
                   
                    dispatcherService.InvokeOnUIThread(() =>
                        {
                            ClockItemsForSelectedClock.Add(model);
                        });
                }

                var clockItemsEvent = clockItems
                    .Where(ci => ci.OrderIndex == -1)
                    .Where(ci => !ci.ClockItemEventId.HasValue)
                    .ToList();

                foreach (var clockItemDto in clockItemsEvent)
                {
                    var model = new ClockItemModel(clockItemDto);
                    int nearestIndex = 0;
                    if (clockItemDto is ClockItemEventDTO eventItem)
                    {
                        var previousItem = ClockItemsForSelectedClock
                            .Where(ci => ci.Item.OrderIndex > -1)
                            .Where(ci => ci.StartTime <= eventItem.EstimatedEventStart)
                            .LastOrDefault();

                        if(previousItem != null)
                        {
                            nearestIndex = ClockItemsForSelectedClock.IndexOf(previousItem) + 1;    
                        }

                        model.StartTime = eventItem.EstimatedEventStart;
                        model.Duration = eventItem.EstimatedEventDuration ?? TimeSpan.Zero;
                    }
                    dispatcherService.InvokeOnUIThread(() =>
                    {
                          ClockItemsForSelectedClock.Insert(nearestIndex, model);
                    });

                    var eventChild = clockItems
                        .Where(ci => ci.OrderIndex == -1)
                        .Where(ci => ci.ClockItemEventId.HasValue)
                        .OrderBy(ci => ci.EventOrderIndex)
                        .ToList();


                    foreach (var clockItemChildDto in eventChild)
                    {
                        if (clockItemChildDto.EventOrderIndex != null)
                        {
                            var childModel = new ClockItemModel(clockItemChildDto);
                            if (clockItemChildDto is ClockItemCategoryDTO category
                        && category.CategoryId.HasValue)
                            {
                                childModel.Duration = categoryAvgDurations[category.CategoryId.Value];


                            }

                            else if (clockItemChildDto is ClockItemTrackDTO trackItem)
                            {
                                childModel.Duration = trackItem.TrackDuration;
                            }
                            dispatcherService.InvokeOnUIThread(() =>
                            {
                                ClockItemsForSelectedClock.Insert(nearestIndex + 1 + clockItemChildDto.EventOrderIndex.Value, 
                                    childModel);
                            });
                        }

                    }

                }
            }
        }

        #endregion

        private void CalculateStartTime()
        {
            TotalDuration = TimeSpan.Zero;
            foreach (var item in ClockItemsForSelectedClock
                .Where(ci => ci.Item.GetType() != typeof(ClockItemEventDTO)))
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
                ClockItemsPieChart.Add(new ClockPieChartModel(clock.DisplayName, (int)clock.Duration.TotalSeconds));
                totalSeconds += (int)clock.Duration.TotalSeconds;
            }
            if (totalSeconds < 3600)
            {
                ClockItemsPieChart.Add(new ClockPieChartModel("Unfilled clock time", 3600 - totalSeconds));
            }
        }

        #region Commands
        [RelayCommand]
        private async void InsertTrackToSelectedClock()
        {
            if (SelectedClock == null) return;
            var vm = windowService.ShowDialog<TrackSelectViewModel>();
           
            if(vm.SelectedTrack == null) return;
            int latestIndex = ClockItemsForSelectedClock.Count;

            ClockItemTrackDTO newClockItem = new ClockItemTrackDTO
            {
                ClockId = SelectedClock.Id,
                OrderIndex = latestIndex,
                TrackId = vm.SelectedTrack.Id,
            };

            await clocksService.AddClockItem(newClockItem);

            //Reload
            _ = LoadClockItemsForSelectedClock();
        }

        [RelayCommand]
        private async void InsertCategoryRuleToSelectedClock()
        {
            if (SelectedClock == null) return;
            var vm = windowService.ShowDialog<PlannerManageClockCategoryRuleViewModel>(SelectedClock.Id);
            if (vm.SelectedCategory == null) return;

            DebugHelper.WriteLine(this, $"To add clock rule - {vm.SelectedCategory.Id}");
            int latestIndex = ClockItemsForSelectedClock.Count;

            await vm.AddClockItem(latestIndex);

            //Reload
            _ = LoadClockItemsForSelectedClock();
        }

        [RelayCommand]
        private async void InsertEventRuleToSelectedClock()
        {
            if (SelectedClock == null) return;
            var vm = windowService.ShowDialog<PlannerManageClockEventRuleViewModel>(SelectedClock.Id);
            if (vm.SelectedEvent == null) return;

            await vm.AddClockItem();

            //Reload
            _ = LoadClockItemsForSelectedClock();
        }

        [RelayCommand]
        private void EditSelectedItemInSelectedClock()
        {
            if(SelectedClock == null || SelectedClockItem == null) return;
            if(SelectedClockItem!.Item == null) return;
            if(SelectedClockItem.Item is ClockItemCategoryDTO itemCategory)
            {
                var vm = windowService.ShowDialog<PlannerManageClockCategoryRuleViewModel>(SelectedClock.Id, itemCategory.Id);

                _ = LoadClockItemsForSelectedClock();
            } else if(SelectedClockItem.Item is ClockItemEventDTO itemEvent)
            {
                var vm = windowService.ShowDialog<PlannerManageClockEventRuleViewModel>(SelectedClock.Id, itemEvent.Id);

                _ = LoadClockItemsForSelectedClock();
            }
            
        }

        [RelayCommand]
        private async void RemoveSelectedItemsInSelectedClock()
        {
            DebugHelper.WriteLine(this, $"Selected clock items to remove: {SelectedClockItems.Count}");

            if (SelectedClockItems.Count == 0) return;

            foreach(var item in SelectedClockItems)
            {
                if(item is ClockItemModel clockItem)
                {
                    DebugHelper.WriteLine(this, $"Selected clock item to remove: {clockItem.DisplayName}");
                    await clocksService.DeleteClockItem(clockItem.Item.Id);
                }
     
            }

            _ = LoadClockItemsForSelectedClock();
        }

        [RelayCommand]
        private void CopySelectedItemsInSelectedClock()
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        private void PasteSelectedItemsInSelectedClock()
        {
            throw new NotImplementedException();
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
            var result = windowService.ShowDialog<PlannerManageClockViewModel>(SelectedClock.Id);
            var dto = ClockModel.ToDto(result.ManagedClock);
            clocksService.UpdateClock(dto);
            _ = LoadClocks();

        }

        [RelayCommand]
        private void DuplicateClockDialog()
        {
            if (SelectedClock == null) return;
            windowService.ShowDialog<PlannerManageClockViewModel>(SelectedClock.Id, true);
        }

        [RelayCommand]
        private void RefreshClocks()
        {
            _ = LoadClocks();
        }
        #endregion
    }
}
