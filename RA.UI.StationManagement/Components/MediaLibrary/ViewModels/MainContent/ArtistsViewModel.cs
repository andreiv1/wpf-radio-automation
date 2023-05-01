using CommunityToolkit.Mvvm.ComponentModel;
using RA.DAL;
using RA.DTO;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.MediaLibrary.ViewModels.MainContent
{
    public partial class ArtistsViewModel : ViewModelBase
    {
        private readonly IArtistsService artistsService;
        private readonly ITracksService tracksService;

        public ObservableCollection<ArtistModel> Artists { get; private set; } = new();

        [ObservableProperty]
        private ArtistModel? selectedArtist;

        public ArtistsViewModel(IArtistsService artistsService, ITracksService tracksService)
        {
            this.artistsService = artistsService;
            this.tracksService = tracksService;
            _ = LoadArtists();
        }

        private async Task LoadArtists()
        {
            var artists = await artistsService.GetArtistsAsync(0, 100);
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
    }
}
