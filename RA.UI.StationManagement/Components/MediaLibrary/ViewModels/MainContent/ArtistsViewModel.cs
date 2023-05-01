using RA.DAL;
using RA.DTO;
using RA.UI.Core.ViewModels;
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

        public ObservableCollection<ArtistDto> Artists { get; private set; } = new();
        public ArtistsViewModel(IArtistsService artistsService)
        {
            this.artistsService = artistsService;
            _ = LoadArtists();
        }

        private async Task LoadArtists()
        {
            var artists = await artistsService.GetArtistsAsync(0, 100);
            foreach(var artist in artists)
            {
                this.Artists.Add(artist);
            }
        }
    }
}
