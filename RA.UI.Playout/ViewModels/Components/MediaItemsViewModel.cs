using CommunityToolkit.Mvvm.ComponentModel;
using RA.DAL;
using RA.DTO;
using RA.UI.Core.Services;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Playout.ViewModels.Components
{
    public partial class MediaItemsViewModel : ViewModelBase
    {
        private readonly IDispatcherService dispatcherService;
        private readonly ITracksService tracksService;

        public MainViewModel MainVm { get; set; }

        #region Properties
        public ObservableCollection<TrackListingDTO> Tracks { get; set; } = new();

        [ObservableProperty]
        private string searchQuery = "";

        [ObservableProperty]
        private int totalTracks = 0;

        [ObservableProperty]
        private int pages;

        private const int tracksPerPage = 100;

        [ObservableProperty]
        private TrackListingDTO? selectedTrack;
        #endregion

        public MediaItemsViewModel(IDispatcherService dispatcherService,
                                   ITracksService tracksService)
        {
            this.dispatcherService = dispatcherService;
            this.tracksService = tracksService;
            _ = LoadTracks(0, 100);
        }

        #region Data fetching
        public async Task LoadTracks(int skip, int take)
        {
            IsMainDataLoading = true;
            Tracks.Clear();
            TotalTracks = await tracksService.GetTrackCountAsync();
            Pages = TotalTracks > 0 ? (TotalTracks - 1) / tracksPerPage + 1 : 0;
            var tracks = await tracksService.GetTrackListAsync(skip, take);

            foreach (var track in tracks.ToList())
            {
                await dispatcherService.InvokeOnUIThreadAsync(() =>
                {
                    Tracks.Add(track);
                });
            }
            IsMainDataLoading = false;
        }
        #endregion
    }
}
