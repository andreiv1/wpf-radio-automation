using CommunityToolkit.Mvvm.ComponentModel;
using RA.DAL;
using RA.DTO;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Dialogs.ArtistSelectDialog
{
    public partial class ArtistSelectViewModel : DialogViewModelBase
    {
        private readonly IArtistsService artistsService;

        public ObservableCollection<ArtistDTO> Artists { get; set; } = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FinishDialogCommand))]
        private ArtistDTO? selectedArtist;
        public ArtistSelectViewModel(IWindowService windowService, IArtistsService artistsService) : base(windowService)
        {
            this.artistsService = artistsService;
            _ = LoadArtists();
        }


        #region Data fetching
        private async Task LoadArtists()
        {
            var artists = await artistsService.GetArtistsAsync(0, 9999);
            foreach(var a in artists)
            {
                Artists.Add(a);
            }
        }
        #endregion

        protected override void CancelDialog()
        {
            SelectedArtist = null;
            base.CancelDialog();
        }
        protected override bool CanFinishDialog()
        {
            return SelectedArtist != null;
        }
    }
}
