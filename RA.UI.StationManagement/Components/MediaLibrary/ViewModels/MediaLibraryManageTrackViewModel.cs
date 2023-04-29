using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL.Interfaces;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.MediaLibrary.ViewModels
{
    public partial class MediaLibraryManageTrackViewModel : WindowViewModelBase
    {
        private readonly int trackId;
        private readonly ITracksService tracksService;
        private readonly IMessageBoxService messageBoxService;
        [ObservableProperty]
        private TrackModel? trackModel;

        #region Constructor
        public MediaLibraryManageTrackViewModel(IWindowService windowService, ITracksService tracksService,
            IMessageBoxService messageBoxService) : base(windowService)
        {
            this.tracksService = tracksService;
            this.messageBoxService = messageBoxService;
            TrackModel = new();
        }
        public MediaLibraryManageTrackViewModel(IWindowService windowService, ITracksService tracksService, IMessageBoxService messageBoxService,
            int trackId) : base(windowService) 
        {
            this.trackId = trackId;
            this.tracksService = tracksService;
            this.messageBoxService = messageBoxService;
            LoadTrack();
        }

        #endregion

        private async void LoadTrack()
        {
            var track = await Task.Run(() => tracksService.GetTrack(trackId));
            TrackModel = TrackModel.FromDto(track);
        }

        #region Commands
        [RelayCommand]
        private void SaveTrack()
        {
            messageBoxService.ShowInfo("Fake save!");
        }

        #endregion


    }
}
