using CommunityToolkit.Mvvm.ComponentModel;
using RA.DAL;
using RA.DAL.Models;
using RA.Database.Models.Enums;
using RA.DTO;
using RA.UI.Core.Services;
using RA.UI.Core.ViewModels;
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

        private const int tracksPerPage = 100;

        [ObservableProperty]
        private TrackListingDTO? selectedTrack;

        public MediaItemsViewModel(IDispatcherService dispatcherService,
                                   ITracksService tracksService)
        {
            this.dispatcherService = dispatcherService;
            this.tracksService = tracksService;
            _ = LoadTracks(0, 100);
        }

        ICollection<TrackFilterCondition>? conditions = new List<TrackFilterCondition>()
        {
            new TrackFilterCondition(FilterLabelType.Status,FilterOperator.Equals,TrackStatus.Enabled),
        };
        //Data fetching
        public async Task LoadTracks(int skip, int take, string query = "")
        {
            Tracks.Clear();
            TotalTracks = await tracksService.GetTrackCountAsync(query, conditions: conditions);
            Pages = TotalTracks > 0 ? (TotalTracks - 1) / tracksPerPage + 1 : 0;
            var tracks = await tracksService.GetTrackListAsync(skip, take, query, conditions: conditions);

            foreach (var track in tracks.ToList())
            {
                Tracks.Add(track);
            }

        }
      
    }
}
