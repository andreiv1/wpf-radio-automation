using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        #region Properties
        public ObservableCollection<ArtistModel> Artists { get; private set; } = new();

        [ObservableProperty]
        private string searchQuery = "";

        [ObservableProperty]
        private ArtistModel? selectedArtist;

        #endregion

        public ArtistsViewModel(IArtistsService artistsService, ITracksService tracksService)
        {
            this.artistsService = artistsService;
            this.tracksService = tracksService;
            _ = LoadArtists();
        }

        #region Data fetching
        private async Task LoadArtists()
        {
            IsMainDataLoading = true;
            var artists = await artistsService.GetArtistsAsync(0, 100);
            foreach(var artist in artists)
            {
                this.Artists.Add(ArtistModel.FromDto(artist));
            }
            IsMainDataLoading = false;
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

        #endregion

        #region Commands

        [RelayCommand]
        private void AddItem()
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        private void OpenItemDetails()
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        private void DeleteItem()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
