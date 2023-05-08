using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DTO;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels.Playlists;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.MainContent
{
    public partial class PlannerPlaylistsViewModel : ViewModelBase
    {
        private readonly IWindowService windowService;
        private readonly IPlaylistsService playlistsService;

        public ObservableCollection<PlaylistListingDTO> PlaylistsToAir { get; set; } = new();

        public PlannerPlaylistsViewModel(IWindowService windowService, IPlaylistsService playlistsService)
        {
            this.windowService = windowService;
            this.playlistsService = playlistsService;
            _ = LoadPlaylistsToAir();
        }

        #region Data fetching
        private async Task LoadPlaylistsToAir()
        {
            await Task.Run(() =>
            {
                foreach (var playlist in playlistsService.GetPlaylistsToAir())
                {
                    PlaylistsToAir.Add(playlist);
                }
            });
        }
        #endregion
        #region Commands
        [RelayCommand]
        private void OpenGeneratePlaylists()
        {
            windowService.ShowDialog<PlannerGeneratePlaylistsViewModel>();
        }
        #endregion
    }
}
