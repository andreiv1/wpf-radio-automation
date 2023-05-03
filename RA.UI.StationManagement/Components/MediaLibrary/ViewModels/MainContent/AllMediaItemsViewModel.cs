using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DTO;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.MediaLibrary.ViewModels.MainContent
{
    public partial class AllMediaItemsViewModel : ViewModelBase
    {
        private readonly IWindowService windowService;
        private readonly IDispatcherService dispatcherService;
        private readonly ITracksService tracksService;

        #region Properties
        public ObservableCollection<TrackListDTO> Items { get; set; } = new();

        [ObservableProperty]
        private string searchQuery = "";

        [ObservableProperty]
        private int totalTracks = 0;

        [ObservableProperty]
        private int pages;

        private const int tracksPerPage = 100;

        [ObservableProperty]
        private TrackListDTO? selectedTrack;

        #endregion

        #region Constructor

        public AllMediaItemsViewModel(IWindowService windowService, IDispatcherService dispatcherService,
            ITracksService tracksService)
        {
            this.windowService = windowService;
            this.dispatcherService = dispatcherService;
            this.tracksService = tracksService;
            _ = LoadTracks(0, tracksPerPage);
        }

        #endregion

        public async Task LoadTracks(int skip, int take)
        {
            IsMainDataLoading = true;
            Items.Clear();
            TotalTracks = await tracksService.GetTrackCountAsync();
            var tracks = await tracksService.GetTrackListAsync(skip, take);
            Pages = TotalTracks > 0 ? (TotalTracks - 1) / tracksPerPage + 1 : 0;
            foreach (var track in tracks)
            {
                dispatcherService.InvokeOnUIThread(() =>
                {
                    Items.Add(track);
                });
            }
            IsMainDataLoading = false;
        }

        #region Commands
        [RelayCommand]
        private void AddItem()
        {
            throw new NotImplementedException();
        }


        [RelayCommand]
        private void ImportItems()
        {
            windowService.ShowWindow<MediaLibraryImportItemsViewModel>();
        }

        [RelayCommand]
        private void EditItem()
        {
            if(SelectedTrack is null)
            {
                return;
            }

            windowService.ShowWindow<MediaLibraryManageTrackViewModel>(SelectedTrack.Id);
        }

        [RelayCommand]
        private void DeleteItem()
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        private void FilterItems()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
