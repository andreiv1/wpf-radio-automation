﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL.Interfaces;
using RA.Dto;
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
        public ObservableCollection<TrackListDto> Items { get; set; } = new();

        [ObservableProperty]
        private int totalTracks;

        [ObservableProperty]
        private int tracksPerPage = 100;

        [ObservableProperty]
        private TrackListDto? selectedTrack;
        public AllMediaItemsViewModel(IWindowService windowService, IDispatcherService dispatcherService,
            ITracksService tracksService)
        {
            this.windowService = windowService;
            this.dispatcherService = dispatcherService;
            this.tracksService = tracksService;
            _ = LoadTracks(0, tracksPerPage);
        }



        private async Task LoadTracks(int skip, int take)
        {
            TotalTracks = await Task.Run(() => tracksService.GetTrackCountAsync());
            var tracks = await Task.Run(() => tracksService.GetTrackListAsync(skip, take));
            Items.Clear();
            foreach (var track in tracks)
            {
                dispatcherService.InvokeOnUIThread(() =>
                {
                    Items.Add(track);
                });
            }
        }

        #region Commands
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
        #endregion

    }
}
