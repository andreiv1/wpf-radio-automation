using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.Models;
using RA.UI.StationManagement.Dialogs.CategorySelectDialog;
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
        private readonly IFileBrowserDialogService fileBrowserDialogService;
        [ObservableProperty]
        private TrackModel? track;

        #region Constructor
        public MediaLibraryManageTrackViewModel(IWindowService windowService, IFileBrowserDialogService fileBrowserDialogService,
            IMessageBoxService messageBoxService, ITracksService tracksService) : base(windowService)
        {
            this.tracksService = tracksService;
            this.fileBrowserDialogService = fileBrowserDialogService;
            this.messageBoxService = messageBoxService;
            Track = new();
        }
        public MediaLibraryManageTrackViewModel(IWindowService windowService, IFileBrowserDialogService fileBrowserDialogService,
            IMessageBoxService messageBoxService, ITracksService tracksService, int trackId) : base(windowService) 
        {
            this.trackId = trackId;
            this.tracksService = tracksService;
            this.fileBrowserDialogService = fileBrowserDialogService;
            this.messageBoxService = messageBoxService;
            LoadTrack();
        }

        #endregion

        private async void LoadTrack()
        {
            var track = await Task.Run(() => tracksService.GetTrack(trackId));
            Track = TrackModel.FromDto(track);
        }

        #region Commands
        [RelayCommand]
        private void PickFile()
        {
            fileBrowserDialogService.Filter = "Audio files (*.mp3;*.flac)|*.mp3;*.flac|All Files (*.*)|*.*";
            fileBrowserDialogService.ShowDialog();
            Track!.FilePath = fileBrowserDialogService.SelectedPath;
            messageBoxService.ShowWarning("To do meta-data updating...");
        }

        [RelayCommand]
        private void MoveFile()
        {
            PickFile();
            messageBoxService.ShowWarning("To do file moving...");
        }

        [RelayCommand]
        private void RemoveFile()
        {
            Track!.FilePath = "";
            messageBoxService.ShowWarning("To do actual file removing...");
        }

        [RelayCommand]
        private void AddCategory()
        {
            var vm = windowService.ShowDialog<CategorySelectViewModel>();
            if(vm.SelectedCategory is null)
            {
                return;
            }
            messageBoxService.ShowInfo($"Selected {vm.SelectedCategory.CategoryId}");
        }

        [RelayCommand]
        private void SaveTrack()
        {
            messageBoxService.ShowInfo("Fake save!");
        }

        #endregion


    }
}
