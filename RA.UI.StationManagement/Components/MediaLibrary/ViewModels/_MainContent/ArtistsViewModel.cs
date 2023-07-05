using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DTO;
using RA.Logic;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.MediaLibrary.ViewModels.MainContent
{
    public partial class ArtistsViewModel : ViewModelBase
    {
        private readonly IArtistsService artistsService;
        private readonly ITracksService tracksService;

        private const int artistsPerPage = 200;
        public ObservableCollection<ArtistModel> Artists { get; private set; } = new();

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
                    _ = LoadArtists(0, artistsPerPage, value);
                }
            });
        }

        [ObservableProperty]
        private ArtistModel? selectedArtist;

        [ObservableProperty]
        private int totalArtists = 0;

        [ObservableProperty]
        private int pages;

        [ObservableProperty]
        private int pageIndex = 0;

        public ArtistsViewModel(IArtistsService artistsService, ITracksService tracksService)
        {
            this.artistsService = artistsService;
            this.tracksService = tracksService;
            _ = LoadArtists(0, 100);
 
        }
        public async Task LoadArtists(int skip, int take, string query = "")
        {
            this.Artists.Clear();
            TotalArtists = await artistsService.GetArtistsCountAsync(skip, take, query);
            Pages = TotalArtists > 0 ? (TotalArtists - 1) / artistsPerPage + 1 : 0;
            var artists = await artistsService.GetArtistsAsync(skip, take, query);
            foreach(var artist in artists)
            {
                this.Artists.Add(ArtistModel.FromDto(artist));
            }
        }

        public async Task LoadTracksForArtist(ArtistModel artist)
        {
            if (SelectedArtist == null) return;
            var tracks = await tracksService.GetTrackListByArtistAsync(artist.Id, 0, 100);

            if(SelectedArtist.Tracks == null)
            {
                SelectedArtist.Tracks = new ObservableCollection<ArtistTrackModel>();
            } else
            {
                SelectedArtist.Tracks.Clear();
            }
            foreach (var t in tracks)
            {

                SelectedArtist.Tracks.Add(ArtistTrackModel.FromDto(t));
            }

        }

        [RelayCommand]
        private void AddItem()
        {
            //throw new NotImplementedException();
        }

        [RelayCommand]
        private void OpenItemDetails()
        {
            //throw new NotImplementedException();
        }

        [RelayCommand]
        private void DeleteItem()
        {
            //throw new NotImplementedException();
        }
    }
}
