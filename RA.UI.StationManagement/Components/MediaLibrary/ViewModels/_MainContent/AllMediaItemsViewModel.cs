using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DAL.Models;
using RA.DTO;
using RA.Logic;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Dialogs.TrackFilterDialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace RA.UI.StationManagement.Components.MediaLibrary.ViewModels.MainContent
{
    public partial class AllMediaItemsViewModel : ViewModelBase
    {
        private readonly IWindowService windowService;
        private readonly IDispatcherService dispatcherService;
        private readonly IFileBrowserDialogService fileBrowserDialogService;
        private readonly IMessageBoxService messageBoxService;
        private readonly ITracksService tracksService;

        private const int tracksPerPage = 100;
        public ObservableCollection<TrackListingDTO> Items { get; set; } = new();

        [ObservableProperty]
        private string searchQuery = "";

        private const int searchDelayMilliseconds = 500; // Set an appropriate delay time

        private CancellationTokenSource? searchQueryToken;
        partial void OnSearchQueryChanged(string value)
        {
            if (searchQueryToken != null)
            {
                searchQueryToken.Cancel();
            }

            searchQueryToken = new CancellationTokenSource();
            var cancellationToken = searchQueryToken.Token;
            Task.Delay(searchDelayMilliseconds, cancellationToken).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully && !cancellationToken.IsCancellationRequested)
                {
                    DebugHelper.WriteLine(this, $"Performing search query: {value}");
                    _ = LoadTracks(0, tracksPerPage, value, FilterConditions);
                }
            });
        }

        [ObservableProperty]
        private int totalTracks = 0;

        [ObservableProperty]
        private int pages;

        [ObservableProperty]
        private int pageIndex = 0;

        [ObservableProperty]
        private TrackListingDTO? selectedTrack;

        public AllMediaItemsViewModel(IWindowService windowService,
                                      IDispatcherService dispatcherService,
                                      IFileBrowserDialogService fileBrowserDialogService,
                                      IMessageBoxService messageBoxService,
                                      ITracksService tracksService)
        {
            this.windowService = windowService;
            this.dispatcherService = dispatcherService;
            this.fileBrowserDialogService = fileBrowserDialogService;
            this.messageBoxService = messageBoxService;
            this.tracksService = tracksService;
        }
        public List<TrackFilterCondition>? FilterConditions { get; private set; }

        [ObservableProperty]
        private bool isFiltersApplied;

        partial void OnIsFiltersAppliedChanged(bool value)
        {
            if (!value)
            {
                _ = LoadTracks(0, tracksPerPage);
                FilterConditions = null;
            }
        }
        public async Task LoadTracks(int skip, int take, string query = "", List<TrackFilterCondition>? conditions = null)
        {
            Items.Clear();
            TotalTracks = await tracksService.GetTrackCountAsync(query, conditions);
            Pages = TotalTracks > 0 ? (TotalTracks - 1) / tracksPerPage + 1 : 0;
            var tracks = await tracksService.GetTrackListAsync(skip, take, query, conditions);
            foreach (var track in tracks.ToList())
            {
                Items.Add(track);
            }
        }

        private void LoadTracksFromStart()
        {
            _ = LoadTracks(0, tracksPerPage, conditions: FilterConditions);
            SearchQuery = string.Empty;
            PageIndex = 0;
            
        }
        
        //Commands
        [RelayCommand]
        private void AddItem()
        {
            windowService.ShowDialog<MediaLibraryManageTrackViewModel>();
            LoadTracksFromStart();
        }


        [RelayCommand]
        private void ImportItems()
        {
            windowService.ShowDialog<MediaLibraryImportItemsViewModel>();
            LoadTracksFromStart();   
        }

        [RelayCommand]
        private void EditItem()
        {
            if (SelectedTrack == null) return;
            windowService.ShowDialog<MediaLibraryManageTrackViewModel>(SelectedTrack.Id);
            LoadTracksFromStart();
        }

        [RelayCommand]
        private async void DeleteItem()
        {
            if (SelectedTrack == null) return;
            var isDeleted = await tracksService.DeleteTrack(SelectedTrack.Id);
            var trackString = $"{(string.IsNullOrWhiteSpace(SelectedTrack.Artists) ? 
                string.Empty : $"{SelectedTrack.Artists} - ")}";
            if (isDeleted)
            {
                messageBoxService.ShowInfo($"Selected track '{trackString}{SelectedTrack.Title}' deleted succesfully!");
                LoadTracksFromStart();
            }
            else
            {
                messageBoxService.ShowError($"Selected track '{trackString}{SelectedTrack.Title}' can't be deleted.\n" +
                    $"It might be used somewhere else (in a clock or playlist).");
            }
            
        }

        [RelayCommand]
        private void FilterItems()
        {
            var vm = windowService.ShowDialog<TrackFilterViewModel>();
            FilterConditions = vm?.Conditions;
            if (FilterConditions?.Count > 0) IsFiltersApplied = true;
            else IsFiltersApplied = false;
            _ = LoadTracks(0, tracksPerPage, conditions: FilterConditions);
        }


        [RelayCommand]
        private void RemoveFilters()
        {
            IsFiltersApplied = false;
        }

    }
}
