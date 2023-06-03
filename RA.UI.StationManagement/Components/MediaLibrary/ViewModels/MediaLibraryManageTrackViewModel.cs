using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.Database.Models.Enums;
using RA.DTO;
using RA.Logic.TrackFileLogic;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.Models;
using RA.UI.StationManagement.Dialogs.ArtistSelectDialog;
using RA.UI.StationManagement.Dialogs.CategorySelectDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        [ObservableProperty]
        private TrackArtistDTO? selectedTrackArtist;

        [ObservableProperty]
        private TrackCategoryDTO? selectedCategory;

        [ObservableProperty]
        private String audioFileFormat;

        [ObservableProperty]
        private String audioFileBitrate;

        [ObservableProperty]
        private String audioFileFrequency;

        private string? fullImagePath;
        public string? FullImagePath
        {
            get => fullImagePath;
            private set => SetProperty(ref fullImagePath, value);
        }

        #region Constructor
        public MediaLibraryManageTrackViewModel(IWindowService windowService,
                                                IFileBrowserDialogService fileBrowserDialogService,
                                                IMessageBoxService messageBoxService,
                                                ITracksService tracksService) : base(windowService)
        {
            this.tracksService = tracksService;
            this.fileBrowserDialogService = fileBrowserDialogService;
            this.messageBoxService = messageBoxService;
            Track = new();
        }
        public MediaLibraryManageTrackViewModel(IWindowService windowService,
                                                IFileBrowserDialogService fileBrowserDialogService,
                                                IMessageBoxService messageBoxService,
                                                ITracksService tracksService,
                                                int trackId) : base(windowService) 
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
            
            if (!string.IsNullOrEmpty(Track.ImageName))
            {
                //TODO
                FullImagePath = $"C:\\Users\\Andrei\\Desktop\\images\\{Track.ImageName}";
            }
            else
            {
                FullImagePath = "pack://application:,,,/RA.UI.Core;component/Resources/Images/track_default_image.png";
            }

            //Load audio file metadata
            if (track.FilePath != null)
            {
                var metadata = TrackMetadataReader.GetAudioFileInfo(track.FilePath);
                AudioFileBitrate = metadata["Bitrate"];
                AudioFileFormat = metadata["FileType"];
                AudioFileFrequency = metadata["Frequency"];
            }
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
            if (vm.SelectedCategory == null) return;
            if (Track!.Categories.Where(c => c.CategoryId == vm.SelectedCategory.CategoryId).Any()) return;

            Track!.Categories?.Add(new TrackCategoryDTO()
            {
                CategoryId = vm.SelectedCategory.CategoryId,
                CategoryName = vm.SelectedCategory.Name,
            });
            //messageBoxService.ShowInfo($"Selected {vm.SelectedCategory.CategoryId}");
        }

        [RelayCommand]
        private void RemoveSelectedCategory()
        {
            if (SelectedCategory == null) return; 
            Track!.Categories?.Remove(SelectedCategory);
        }

        [RelayCommand]
        private void AddArtist()
        {
            var vm = windowService.ShowDialog<ArtistSelectViewModel>();
            if(vm.SelectedArtist == null) return;
            if (Track.Artists.Where(a => a.ArtistId == vm.SelectedArtist.Id).Any()) return;
            int orderIndex = 0;
            if (Track.Artists.Any())
            {
                orderIndex = Track.Artists
                    .OrderBy(a => a.OrderIndex)
                    .Last().OrderIndex;
                orderIndex++;
            }
            Track.Artists.Add(new TrackArtistDTO()
            {
                ArtistId = vm.SelectedArtist.Id,
                ArtistName = vm.SelectedArtist.Name,
                OrderIndex = orderIndex,
            });
        }

        [RelayCommand]
        private void RemoveArtist()
        {
            if (SelectedTrackArtist == null) return;
            Track.Artists.Remove(SelectedTrackArtist);
            for(int i = 0; i < Track.Artists.Count; i++)
            {
                Track.Artists.ElementAt(i).OrderIndex = i;
            }
        }

        [RelayCommand]
        private void SaveTrack()
        {
            //messageBoxService.ShowInfo("Fake save!");
            if(Track != null)
            tracksService.UpdateTrack(TrackModel.ToDto(Track));
        }

        #endregion

    }
}
