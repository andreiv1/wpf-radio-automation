﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DTO;
using RA.Logic;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels.Clocks;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent.Models;
using RA.UI.StationManagement.Dialogs.TrackSelectDialog;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.MainContent
{
    public partial class PlannerClocksViewModel : ViewModelBase
    {
        private readonly IClocksService clocksService;
        private readonly IWindowService windowService;
        private readonly IMessageBoxService messageBoxService;
        private readonly IDispatcherService dispatcherService;

        public ObservableCollection<ClockDTO> Clocks { get; set; } = new();

        public ObservableCollection<ClockItemModel> ClockItemsForSelectedClock { get; set; } = new();

        public ObservableCollection<ClockPieChartModel> ClockItemsPieChart { get; set; } = new();

        [ObservableProperty]
        private ClockDTO? selectedClock = null;

        [ObservableProperty]
        private ClockItemModel? selectedClockItem = null;

        [ObservableProperty]
        private bool isRuleSelectionEnabled = false;
        public ObservableCollection<object> SelectedClockItems { get; set; } = new();
        partial void OnSelectedClockChanged(ClockDTO? value)
        {
            _ = LoadClockItemsForSelectedClock();
            SelectedClockItem = null;
            ClockItemsForSelectedClock.Clear();
            SelectedClockItems.Clear();
            IsRuleSelectionEnabled = true;
            NotifyAllHeaderButtons();
        }
        [ObservableProperty]
        private TimeSpan totalDuration;

        [ObservableProperty]
        private string searchQuery = "";

        private const int searchDelayMilliseconds = 500; // Set an appropriate delay time

        private CancellationTokenSource? searchQueryToken;
        partial void OnSearchQueryChanged(string value)
        {
            if (searchQueryToken != null)
            {
                searchQueryToken.Cancel();
            }

            searchQueryToken = new CancellationTokenSource();
            var cancellationToken = searchQueryToken.Token;
            Task.Delay(searchDelayMilliseconds, cancellationToken).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully && !cancellationToken.IsCancellationRequested)
                {
                    DebugHelper.WriteLine(this, $"Performing search query: {value}");
                    _ = LoadClocks(value);
                }
            });
        }
        public PlannerClocksViewModel(IWindowService windowService,
                                      IMessageBoxService messageBoxService,
                                      IDispatcherService dispatcherService,
                                      IClocksService clocksService)
        {
            this.windowService = windowService;
            this.messageBoxService = messageBoxService;
            this.dispatcherService = dispatcherService;
            this.clocksService = clocksService;
            _ = LoadClocks();
            ClockItemsForSelectedClock.CollectionChanged += ClockItemsForSelectedClock_CollectionChanged;
        }

        private void ClockItemsForSelectedClock_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CalculateStartTime();
            FillPieChart();
        }

        //Data fetching
        private async Task LoadClocks(string? query = null)
        {
            var clocks = await Task.Run(() => clocksService.GetClocksAsync(query));
            if (string.IsNullOrEmpty(query)) SearchQuery = string.Empty;
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

                var clockItemsEvent = clockItems
                    .Where(ci => ci.OrderIndex == -1)
                    .Where(ci => !ci.ClockItemEventId.HasValue)
                    .ToList();

                foreach (var clockItemDto in clockItemsNormal)
                {
                    var model = new ClockItemModel(clockItemDto);
                    if (clockItemDto is ClockItemCategoryDTO category
                        && category.CategoryId.HasValue)
                    {
                        model.Duration = categoryAvgDurations[category.CategoryId.Value];
                    }

                    else if (clockItemDto is ClockItemTrackDTO trackItem)
                    {
                        model.Duration = trackItem.TrackDuration;
                    }

                    dispatcherService.InvokeOnUIThread(() =>
                    {
                        ClockItemsForSelectedClock.Add(model);
                    });
                }

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

                        if (previousItem != null)
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
                        .Where(ci => ci.ClockItemEventId == clockItemDto.Id)
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


        private void CalculateStartTime()
        {
            TotalDuration = TimeSpan.Zero;

            foreach (var model in ClockItemsForSelectedClock)
            {
                if (model.Item is ClockItemCategoryDTO categoryItem
                    && !model.Item.ClockItemEventId.HasValue)
                {
                    model.StartTime = TotalDuration;
                    TotalDuration += model.Duration;
                }
                else if (model.Item is ClockItemTrackDTO trackItem
                    && !model.Item.ClockItemEventId.HasValue)
                {
                    model.StartTime = TotalDuration;
                    TotalDuration += model.Duration;

                }
                else if (model.Item is ClockItemEventDTO eventItem)
                {
                    model.StartTime = eventItem.EstimatedEventStart;
                    model.Duration = TimeSpan.Zero;
                }

                if (model.Item.ClockItemEventId.HasValue)
                {
                    var eventItem = ClockItemsForSelectedClock
                        .Where(ci => ci.Item.Id == model.Item.ClockItemEventId.Value)
                                   .FirstOrDefault();
                    if (eventItem != null)
                    {
                        eventItem.Duration += model.Duration;
                    }
                }
            }
        }
        private void FillPieChart()
        {
            ClockItemsPieChart.Clear();
            int totalSeconds = 0;
            foreach (var clock in ClockItemsForSelectedClock)
            {
                int duration = (int)clock.Duration.TotalSeconds;
                if (!clock.Item.ClockItemEventId.HasValue && duration > 10)
                {
                    ClockItemsPieChart.Add(new ClockPieChartModel(clock.DisplayName, duration));
                    totalSeconds += duration;
                }
            }
            if (totalSeconds <= 3600)
            {
                ClockItemsPieChart.Add(new ClockPieChartModel("Unfilled clock time", 3600 - totalSeconds));
            }
        }

        //Commands
        [RelayCommand]
        private async void InsertTrackToSelectedClock()
        {
            if (SelectedClock == null) return;
            var vm = windowService.ShowDialog<TrackSelectViewModel>();

            if (vm.SelectedTrack == null) return;
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

            int latestIndex = ClockItemsForSelectedClock
                .Where(x => x.Item.OrderIndex > -1)
                .Count();

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

        [RelayCommand(CanExecute = nameof(CanUseHeaderButtons))]
        private async void EditSelectedItemInSelectedClock()
        {
            if (SelectedClock == null || SelectedClockItem == null) return;
            if (SelectedClockItem!.Item == null) return;
            if (SelectedClockItem.Item is ClockItemCategoryDTO itemCategory)
            {
                var vm = windowService.ShowDialog<PlannerManageClockCategoryRuleViewModel>(SelectedClock.Id, itemCategory.Id);

                if (vm != null)
                {
                    await vm.UpdateClockItem();
                }
            }
            else if (SelectedClockItem.Item is ClockItemEventDTO itemEvent)
            {
                var vm = windowService.ShowDialog<PlannerManageClockEventRuleViewModel>(SelectedClock.Id, itemEvent.Id);
                messageBoxService.ShowWarning("TO DO UPDATE");
            }
            else if (SelectedClockItem.Item is ClockItemTrackDTO itemTrack)
            {
                messageBoxService.ShowWarning("TO DO");
            }
            _ = LoadClockItemsForSelectedClock();
        }

        [RelayCommand(CanExecute = nameof(CanUseHeaderButtons))]
        private async void RemoveSelectedItemsInSelectedClock()
        {
            DebugHelper.WriteLine(this, $"Selected clock items to remove: {SelectedClockItems.Count}");

            if (SelectedClockItems.Count == 0) return;

            foreach (var item in SelectedClockItems)
            {
                if (item is ClockItemModel clockItem)
                {
                    DebugHelper.WriteLine(this, $"Selected clock item to remove: {clockItem.DisplayName}");
                    await clocksService.DeleteClockItem(clockItem.Item.Id);
                }

            }

            _ = LoadClockItemsForSelectedClock();
        }

        [RelayCommand]
        private async void DuplicateItemsInSelectedClock()
        {
            if (SelectedClockItems.Count == 0 || SelectedClock == null) return;
            var ids = SelectedClockItems.Select(x => ((ClockItemModel)x).Item.Id).ToList();
            await clocksService.DuplicateClockItems(ids, SelectedClock.Id);
            await LoadClockItemsForSelectedClock();
            SelectedClockItems.Clear();
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
            SearchQuery = string.Empty;
            _ = LoadClocks();
           

        }

        [RelayCommand]
        private void DuplicateClockDialog()
        {
            if (SelectedClock == null) return;
            windowService.ShowDialog<PlannerManageClockViewModel>(SelectedClock.Id, true);
        }

        [RelayCommand]
        private async void RemoveSelectedClock()
        {
            if (SelectedClock == null) return;
            var result = await clocksService.RemoveClock(SelectedClock.Id);
            if (result)
            {
                messageBoxService.ShowInfo($"Clock '{SelectedClock.Name}' deleted succcesfully.'");
                SelectedClock = null;
                SearchQuery = string.Empty;
                _ = LoadClocks();
            }
            else
            {
                messageBoxService.ShowError($"Clock '{SelectedClock.Name}' couldn't be deleted.");
            }
        }

        [RelayCommand]
        private void RefreshClocks()
        {
            _ = LoadClocks();
            SearchQuery = string.Empty;
            IsRuleSelectionEnabled = false;
        }

        [RelayCommand]
        private async void InsertTrackInSelectedEvent()
        {
            if (SelectedClockItem == null || SelectedClock == null) return;

            var isEvent = SelectedClockItem.Item is ClockItemEventDTO;
            if (!isEvent || SelectedClockItem.Item == null)
            {
                messageBoxService.ShowWarning($"You can attach a track only to an event!");
                return;
            }
            if (SelectedClockItem.Item.Id == null) return;

            var vm = windowService.ShowDialog<TrackSelectViewModel>();
            if (vm.SelectedTrack == null) return;
            var latestEventIndex = ClockItemsForSelectedClock?
                            .Where(ci => ci.Item.OrderIndex == -1)
                            .Where(ci => ci.Item.ClockItemEventId == SelectedClockItem.Item.Id)
                            .Where(ci => ci.Item.EventOrderIndex != null)
                            .Select(x => x.Item.EventOrderIndex)
                            .LastOrDefault();

            int newIndex = latestEventIndex != null ? latestEventIndex.Value + 1 : 0;

            ClockItemTrackDTO newClockItem = new ClockItemTrackDTO
            {
                ClockId = SelectedClock.Id,
                OrderIndex = -1,
                ClockItemEventId = SelectedClockItem.Item.Id,
                EventOrderIndex = newIndex,
                TrackId = vm.SelectedTrack.Id,
            };

            await clocksService.AddClockItem(newClockItem);

            //Reload
            _ = LoadClockItemsForSelectedClock();
        }

        [RelayCommand]
        private async void InsertCategoryInSelectedEvent()
        {
            if (SelectedClockItem == null || SelectedClock == null) return;

            var isEvent = SelectedClockItem.Item is ClockItemEventDTO;
            if (!isEvent || SelectedClockItem.Item == null)
            {
                messageBoxService.ShowWarning($"You can attach a category selection rule only to an event!");
                return;
            }
            if (SelectedClockItem.Item.Id == null) return;
            var latestEventIndex = ClockItemsForSelectedClock?
                            .Where(ci => ci.Item.OrderIndex == -1)
                            .Where(ci => ci.Item.ClockItemEventId == SelectedClockItem.Item.Id)
                            .Where(ci => ci.Item.EventOrderIndex != null)
                            .Select(x => x.Item.EventOrderIndex)
                            .LastOrDefault();
            int newIndex = latestEventIndex != null ? latestEventIndex.Value + 1 : 0;
            var vm = windowService.ShowDialog<PlannerManageClockCategoryRuleViewModel>(SelectedClock.Id);
            await vm.AddClockItemToEvent(newIndex, SelectedClockItem.Item.Id);

            _ = LoadClockItemsForSelectedClock();
        }

        [RelayCommand(CanExecute = nameof(CanUseHeaderButtons))]
        private void PreviewClock()
        {
            if (SelectedClock == null) return;
            windowService.ShowDialog<PlannerPreviewClockViewModel>();
        }

        private bool CanUseHeaderButtons()
        {
            return SelectedClock != null;
        }

        private void NotifyAllHeaderButtons()
        {
            PreviewClockCommand.NotifyCanExecuteChanged();
            EditSelectedItemInSelectedClockCommand.NotifyCanExecuteChanged();
            RemoveSelectedItemsInSelectedClockCommand.NotifyCanExecuteChanged();
        }




    }
}
