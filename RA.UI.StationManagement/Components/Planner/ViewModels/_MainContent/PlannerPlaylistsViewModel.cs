using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DTO;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels._MainContent.Models;
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

        [ObservableProperty]
        private DateTime selectedDate = DateTime.Now.Date;

        partial void OnSelectedDateChanged(DateTime value)
        {
            _ = LoadPlaylistsByHour(value);
        }

        public ObservableCollection<PlaylistByHourModel> PlaylistsByHour { get; set; } = new();

        public PlannerPlaylistsViewModel(IWindowService windowService, IPlaylistsService playlistsService)
        {
            this.windowService = windowService;
            this.playlistsService = playlistsService;
            _ = LoadPlaylistsToAir();
            _ = LoadPlaylistsByHour(SelectedDate);
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

        private async Task LoadPlaylistsByHour(DateTime date)
        {
            await Task.Run(() =>
            {
                PlaylistsByHour.Clear();
                var data = playlistsService.GetPlaylistsByHour(date)
                    .Select(p => PlaylistByHourModel.FromDTO(p));
                foreach (var item in data)
                {
                    PlaylistsByHour.Add(item);
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

        [RelayCommand]
        private void GoPreviousDate()
        {
            SelectedDate = SelectedDate.AddDays(-1);
        }

        [RelayCommand]
        private void GoNextDate()
        {
            SelectedDate = SelectedDate.AddDays(1);
        }
        #endregion
    }
}
