using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DTO;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels._MainContent.Models;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent.Models;
using RA.UI.StationManagement.Components.Planner.ViewModels.Playlists;
using RA.UI.StationManagement.Dialogs.TrackSelectDialog;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.MainContent
{
    public partial class PlannerPlaylistsViewModel : ViewModelBase
    {
        private readonly IWindowService windowService;
        private readonly IPlaylistsService playlistsService;
        public ObservableCollection<PlaylistListingDTO> PlaylistsToAir { get; set; } = new();

        [ObservableProperty]
        private bool isPlaylistSelected = false;

        [ObservableProperty]
        private PlaylistListingDTO? selectedPlaylistToAir;

        partial void OnSelectedPlaylistToAirChanging(PlaylistListingDTO? value)
        {
            IsPlaylistSelected = value != null;
        }

        [ObservableProperty]
        private DateTime selectedDate = DateTime.Now.Date;

        public ObservableCollection<PlaylistByHourModel> PlaylistsByHour { get; set; } = new();

        public ObservableCollection<PlaylistItemModel> SelectedPlaylistItems { get; set; } = new();

        [ObservableProperty]
        private PlaylistItemModel? selectedPlaylistItem;

        public PlannerPlaylistsViewModel(IWindowService windowService, IPlaylistsService playlistsService)
        {
            this.windowService = windowService;
            this.playlistsService = playlistsService;
            _ = LoadPlaylistsToAir();
            _ = LoadPlaylistsByHour(SelectedDate);
            SelectedPlaylistItems.CollectionChanged += SelectedPlaylistItems_CollectionChanged;
        }

        private void SelectedPlaylistItems_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        partial void OnSelectedDateChanged(DateTime value)
        {
            _ = LoadPlaylistsByHour(value);
        }

        partial void OnSelectedPlaylistToAirChanged(PlaylistListingDTO? value)
        {
            _ = LoadSelectedPlaylistItems();
            IsPlaylistSelected = true;
        }

        //Data fetching
        private async Task LoadPlaylistsToAir()
        {
            PlaylistsToAir.Clear();
            await Task.Run(async () =>
            {
                foreach (var playlist in await playlistsService.GetPlaylistsToAirAfterDate())
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

        private async Task LoadSelectedPlaylistItems()
        {
            if (SelectedPlaylistToAir == null) return;
            await Task.Run(async () =>
            {
                SelectedPlaylistItems.Clear();
                var data = await playlistsService.GetPlaylistItems(SelectedPlaylistToAir.Id);
                int index = 0;
                foreach (var item in data)
                {
                    if (item.GetType() == typeof(PlaylistItemDTO))
                    {
                        PlaylistItemDTO? dto = item as PlaylistItemDTO;
                        
                        if (dto != null)
                        {
                            PlaylistItemModel model = PlaylistItemModel.FromDTO(dto);
                            model.UpdateItemDetails();
                            model.Index = index++;
                            SelectedPlaylistItems.Add(model);
                            
                        }
                    }
                }
            });
        }
   
        // Commands
        [RelayCommand]
        private void OpenGeneratePlaylists()
        {
            windowService.ShowDialog<PlannerGeneratePlaylistsViewModel>();
            _ = LoadPlaylistsToAir();
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


        [RelayCommand]
        private void DeleteSelectedPlaylist()
        {
            if(SelectedPlaylistToAir == null) return;
            int id = SelectedPlaylistToAir.Id;
            playlistsService.DeletePlaylist(id);
            SelectedPlaylistToAir = null;
            _ = LoadPlaylistsToAir();

        }

        //Selected playlist operations
        [RelayCommand]
        private void InsertTrackToPlaylist()
        {
            var vm = windowService.ShowDialog<TrackSelectViewModel>();
            if (SelectedPlaylistToAir != null && vm.SelectedTrack != null)
            {
                PlaylistItemDTO newItem = new()
                {
                    PlaylistId = SelectedPlaylistToAir.Id,
                    Track = new TrackListingDTO() { Id = vm.SelectedTrack.Id }
                };
                playlistsService.AddPlaylistItem(newItem);
                _ = LoadSelectedPlaylistItems();
            }
        }

        [RelayCommand] 
        private void InsertMarkerToPlaylist(object? param)
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        private void DeleteItemFromPlaylist()
        {
            if(SelectedPlaylistItem != null)
            {
                playlistsService.DeletePlaylistItem(SelectedPlaylistItem.Id);
                _ = LoadSelectedPlaylistItems();
            }
        }

        [RelayCommand]
        private void RegenerateSelectedPlaylist()
        {
              throw new NotImplementedException();
        }

        public void MoveItem(int originalIndex, int newIndex)
        {
            var item = SelectedPlaylistItems.ElementAt(newIndex);
        }
       
    } 
}
