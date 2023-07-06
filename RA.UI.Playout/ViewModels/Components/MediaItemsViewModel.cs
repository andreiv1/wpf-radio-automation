using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DAL.Models;
using RA.Database.Models.Enums;
using RA.DTO;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.Playout.Dialogs.TrackFilterDialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RA.UI.Playout.ViewModels.Components
{
    public partial class MediaItemsViewModel : ViewModelBase
    {
        private readonly IWindowService windowService;
        private readonly IDispatcherService dispatcherService;
        private readonly ITracksService tracksService;
        public MainViewModel? MainVm { get; set; }
        public ObservableCollection<TrackListingDTO> Tracks { get; set; } = new();

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
                    _ = LoadTracks(0, tracksPerPage, value);
                }
            });
        }

        [ObservableProperty]
        private int totalTracks = 0;

        [ObservableProperty]
        private int pages;

        [ObservableProperty]
        private int pageIndex = 0;

        private const int tracksPerPage = 100;

        public readonly static List<TrackFilterCondition> defaultFilterConditions = new()
        {
            new TrackFilterCondition(FilterLabelType.Status,FilterOperator.Equals,TrackStatus.Enabled),
        };
        public List<TrackFilterCondition>? FilterConditions { get; private set; } = defaultFilterConditions;

        [ObservableProperty]
        private bool isFiltersApplied;

        partial void OnIsFiltersAppliedChanged(bool value)
        {
            if (!value)
            {
                FilterConditions = defaultFilterConditions;
                _ = LoadTracks(0, tracksPerPage, SearchQuery);
            }
        }

        [ObservableProperty]
        private TrackListingDTO? selectedTrack;

        public MediaItemsViewModel(IWindowService windowService,
                                   IDispatcherService dispatcherService,
                                   ITracksService tracksService)
        {
            this.windowService = windowService;
            this.dispatcherService = dispatcherService;
            this.tracksService = tracksService;
            _ = LoadTracks(0, 100);
        }
        //Data fetching
        public async Task LoadTracks(int skip, int take, string query = "")
        {
            Tracks.Clear();
            TotalTracks = await tracksService.GetTrackCountAsync(query, conditions: FilterConditions);
            Pages = TotalTracks > 0 ? (TotalTracks - 1) / tracksPerPage + 1 : 0;
            var tracks = await tracksService.GetTrackListAsync(skip, take, query, conditions: FilterConditions);

            foreach (var track in tracks.ToList())
            {
                Tracks.Add(track);
            }
        }

        //Commands
        [RelayCommand]
        private void FilterItems()
        {
            var vm = windowService.ShowDialog<TrackFilterViewModel>();
            FilterConditions = vm?.Conditions;
            FilterConditions?.Add(new TrackFilterCondition(FilterLabelType.Status, FilterOperator.Equals, TrackStatus.Enabled));
            if (FilterConditions?.Count > 1) IsFiltersApplied = true;
            else IsFiltersApplied = false;
            _ = LoadTracks(0, tracksPerPage);
        }

        [RelayCommand]
        private void RemoveFilters()
        {
            IsFiltersApplied = false;
        }


    }
}
