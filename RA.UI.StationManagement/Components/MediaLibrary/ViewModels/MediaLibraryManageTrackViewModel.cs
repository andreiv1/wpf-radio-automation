using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DTO;
using RA.Logic.Tracks;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.Models;
using RA.UI.StationManagement.Dialogs.ArtistSelectDialog;
using RA.UI.StationManagement.Dialogs.CategorySelectDialog;
using RA.UI.StationManagement.Stores;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.MediaLibrary.ViewModels
{
    public partial class MediaLibraryManageTrackViewModel : WindowViewModelBase
    {
        private readonly int trackId;
        private readonly ITracksService tracksService;
        private readonly ITagsService tagsService;
        private readonly ConfigurationStore configurationStore;
        private readonly IMessageBoxService messageBoxService;
        private readonly IFileBrowserDialogService fileBrowserDialogService;
        private readonly IDispatcherService dispatcherService;
        [ObservableProperty]
        private TrackModel? track;

        [ObservableProperty]
        private TrackArtistDTO? selectedTrackArtist;

        [ObservableProperty]
        private TrackCategoryDTO? selectedCategory;

        [ObservableProperty]
        private String audioFileFormat = string.Empty;

        [ObservableProperty]
        private String audioFileBitrate = string.Empty;

        [ObservableProperty]
        private String audioFileFrequency = string.Empty;

        private string? fullImagePath;
        public string? FullImagePath
        {
            get => fullImagePath;
            private set => SetProperty(ref fullImagePath, value);
        }
        public ObservableCollection<TagValueDTO> Genres { get; set; } = new();
        public ObservableCollection<TagValueDTO> SelectedGenres { get; set; } = new();
        public ObservableCollection<TagValueDTO> Languages { get; set; } = new();
        public ObservableCollection<TagValueDTO> SelectedLanguages { get; set; } = new();
        public ObservableCollection<TagValueDTO> Moods { get; set; } = new();
        public ObservableCollection<TagValueDTO> SelectedMoods { get; set; } = new();

        //Create constructor
        public MediaLibraryManageTrackViewModel(IWindowService windowService,
                                                IFileBrowserDialogService fileBrowserDialogService,
                                                IDispatcherService dispatcherService,
                                                IMessageBoxService messageBoxService,
                                                ITracksService tracksService,
                                                ITagsService tagsService,
                                                 ConfigurationStore configurationStore) : base(windowService)
        {
            this.tracksService = tracksService;
            this.tagsService = tagsService;
            this.configurationStore = configurationStore;
            this.fileBrowserDialogService = fileBrowserDialogService;
            this.dispatcherService = dispatcherService;
            this.messageBoxService = messageBoxService;
        
            var fetchTagsTask = FetchTags();

            SelectedGenres.CollectionChanged += SelectedGenres_CollectionChanged;
            SelectedLanguages.CollectionChanged += SelectedLanguages_CollectionChanged;
            SelectedMoods.CollectionChanged += SelectedMoods_CollectionChanged;

            fullImagePath = "pack://application:,,,/RA.UI.Core;component/Resources/Images/track_default_image.png";
            
        }

        //Edit constructor
        public MediaLibraryManageTrackViewModel(IWindowService windowService,
                                                IFileBrowserDialogService fileBrowserDialogService,
                                                IDispatcherService dispatcherService,
                                                IMessageBoxService messageBoxService,
                                                ITracksService tracksService,
                                                ITagsService tagsService,
                                                ConfigurationStore configurationStore,
                                                int trackId) : base(windowService) 
        {
            this.trackId = trackId;
            this.tracksService = tracksService;
            this.tagsService = tagsService;
            this.configurationStore = configurationStore;
            this.fileBrowserDialogService = fileBrowserDialogService;
            this.dispatcherService = dispatcherService;
            this.messageBoxService = messageBoxService;
            var loadTask = LoadTrack();
            var fetchTagsTask = FetchTags();

            Task.WhenAll(loadTask, fetchTagsTask)
                .ContinueWith(t => AsignTagsToTrack());

            SelectedGenres.CollectionChanged += SelectedGenres_CollectionChanged;
            SelectedLanguages.CollectionChanged += SelectedLanguages_CollectionChanged;
            SelectedMoods.CollectionChanged += SelectedMoods_CollectionChanged;
        }

        private void SelectedMoods_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ProcessTags_CollectionChnaged(e);
        }

        private void SelectedLanguages_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ProcessTags_CollectionChnaged(e);
        }

        private void SelectedGenres_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ProcessTags_CollectionChnaged(e);
        }

        private void ProcessTags_CollectionChnaged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                var removedItems = e.OldItems;
                if (removedItems == null || Track == null || Track.Tags == null) 
                    return;

                foreach (var item in removedItems)
                {
                    TagValueDTO? tagValue = item as TagValueDTO;
                    if (tagValue != null)
                    {
                        var toDelete = Track.Tags.Where(t => t.TagValueId == tagValue.Id).First();
                        Track.Tags.Remove(toDelete);
                    }
                }
            } else if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                var addedItems = e.NewItems;
                if (addedItems == null || Track == null || Track.Tags == null)
                    return;

                foreach (var item in addedItems)
                {
                    TagValueDTO? tagValue = item as TagValueDTO;
                    if (tagValue != null)
                    {
                        var alreadyExists = Track.Tags.Any(t => t.TagValueId == tagValue.Id);
                        if(!alreadyExists)
                        {
                            Track.Tags.Add(new TrackTagDTO() { TagCategoryId = tagValue.TagCategoryId, TagValueId = tagValue.Id, TrackId = Track.Id });
                        }
                        
                    }
                }
            }
        }

        private async Task LoadTrack()
        {
            var track = await Task.Run(() => tracksService.GetTrack(trackId));
            Track = TrackModel.FromDto(track);
            
            if (!string.IsNullOrEmpty(Track.ImageName))
            {
                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                FullImagePath = configurationStore.GetFullImagePath(Track.ImageName);
            }
            else
            {
                FullImagePath = ConfigurationStore.GetDefaultImagePath();
            }

            //Load audio file metadata
            _ = Task.Run(async () =>
            {
                if (track.FilePath != null)
                {
                    var metadata = await TrackMetadataReader.GetAudioFileInfo(track.FilePath);
                    AudioFileBitrate = metadata["Bitrate"];
                    AudioFileFormat = metadata["FileType"];
                    AudioFileFrequency = metadata["Frequency"];
                }
            });
            
        }

        private async Task FetchTags()
        {
            foreach (var genre in await tagsService.GetTagValuesByCategoryNameAsync("Genre"))
            {
                Genres.Add(genre);
            }

            foreach (var language in await tagsService.GetTagValuesByCategoryNameAsync("Language"))
            {
                Languages.Add(language);
            }

            foreach (var mood in await tagsService.GetTagValuesByCategoryNameAsync("Mood"))
            {
                Moods.Add(mood);
            }
        }

        private void AsignTagsToTrack()
        {
            if (Track == null || Track.Tags == null) throw new Exception("t or tt can't be null");

            foreach (var tagValue in Track.Tags)
            {
                //Genre
                switch (tagValue.TagCategoryId)
                {
                    case 1:
                        var genre = Genres.Where(g => g.Id == tagValue.TagValueId).FirstOrDefault();
                        if (genre != null)
                        {
                            dispatcherService.InvokeOnUIThread(() => SelectedGenres.Add(genre));
                        }
                        break;
                    case 2:
                        var language = Languages.Where(g => g.Id == tagValue.TagValueId).FirstOrDefault();
                        if (language != null)
                        {
                            dispatcherService.InvokeOnUIThread(() => SelectedLanguages.Add(language));
                        }
                        break;
                    case 3:
                        var mood = Moods.Where(g => g.Id == tagValue.TagValueId).FirstOrDefault();
                        if (mood != null)
                        {
                            dispatcherService.InvokeOnUIThread(() => SelectedMoods.Add(mood));
                        }
                        break;
                }
            }
        }

        //Commands
        [RelayCommand]
        private void PickFile()
        {
            fileBrowserDialogService.Filter = "Audio files (*.mp3;*wav;*.flac)|*.mp3;*wav;*.flac|All Files (*.*)|*.*";
            fileBrowserDialogService.ShowDialog();
            Track!.FilePath = fileBrowserDialogService.SelectedPath;
            messageBoxService.ShowWarning("To do metadata updating...");
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
            if (vm.SelectedCategory == null || Track == null) return;
            if (Track.Categories == null) Track.Categories = new();
            if (Track.Categories.Any(c => c.CategoryId == vm.SelectedCategory.CategoryId)) return;

            Track.Categories.Add(new TrackCategoryDTO()
            {
                CategoryId = vm.SelectedCategory.CategoryId,
                CategoryName = vm.SelectedCategory.Name,
            });
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
            if (Track?.Artists?.Any(a => a.ArtistId == vm.SelectedArtist.Id) ?? true) return;
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
                ArtistName = vm.SelectedArtist?.Name ?? "Artist Name",
                OrderIndex = orderIndex,
            });
        }

        [RelayCommand]
        private void RemoveArtist()
        {
            if (SelectedTrackArtist == null || Track == null || Track.Artists == null) return;
            Track?.Artists?.Remove(SelectedTrackArtist);
            for(int i = 0; i < Track!.Artists.Count; i++)
            {
                Track.Artists.ElementAt(i).OrderIndex = i;
            }
        }

        [RelayCommand]
        private void SaveTrack()
        {
            if (Track == null) return;
            var dto = TrackModel.ToDto(Track);
            if (Track != null) tracksService.UpdateTrack(dto);
            var updateTime = DateTime.Now;
            messageBoxService.ShowYesNoInfo(
                message: $"Media item saved succesfully. Would you like to exit?", 
                title: "Item info",
                actionYes: () => { windowService.CloseDialog(); }, 
                actionNo: () => { Track!.DateModified = updateTime; });
        }

        

    }
}
