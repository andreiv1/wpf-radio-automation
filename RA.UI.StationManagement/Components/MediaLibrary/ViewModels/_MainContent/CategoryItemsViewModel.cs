using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DTO;
using RA.Logic;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.Categories;
using RA.UI.StationManagement.Services;
using Syncfusion.Windows.Controls.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace RA.UI.StationManagement.Components.MediaLibrary.ViewModels.MainContent
{
    public partial class CategoryItemsViewModel : ViewModelBase
    {
        private readonly IWindowService windowService;
        private readonly IMessageBoxService messageBoxService;
        private readonly ICategoriesService categoryService;
        private readonly ITracksService tracksService;
        private readonly INavigationService<MediaLibraryMainViewModel> navigationService;
        private readonly MediaLibraryTreeMenuService treeMenuService;
        private readonly int categoryId;
        public ObservableCollection<CategoryDTO> Subcategories { get; private set; } = new();

        public ObservableCollection<TrackListingDTO> CategoryTracks { get; private set; } = new();


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
                    //_ = LoadTracks(0, tracksPerPage, value);
                }
            });
        }

        [ObservableProperty]
        private CategoryDTO? selectedSubcategory;

        [ObservableProperty]
        private CategoryHierarchyDTO? categoryHierarchy;

        [ObservableProperty]
        private TrackListingDTO? selectedTrack;

        [ObservableProperty]
        private bool hasTracks = false;

        [ObservableProperty]
        private bool hasSubcategories = false;

        private const int tracksPerPage = 100;

        [ObservableProperty]
        private int totalTracks = 0;

        [ObservableProperty]
        private int pages;

        [ObservableProperty]
        private int pageIndex = 0;

        [ObservableProperty]
        private TimeSpan avgDuration = TimeSpan.Zero;

        public CategoryItemsViewModel(IWindowService windowService,
                                      IMessageBoxService messageBoxService,
                                      ICategoriesService categoryService,
                                      ITracksService tracksService,
                                      INavigationService<MediaLibraryMainViewModel> navigationService,
                                      MediaLibraryTreeMenuService treeMenuService,
                                      int categoryId)
        {
            this.windowService = windowService;
            this.messageBoxService = messageBoxService;
            this.categoryService = categoryService;
            this.tracksService = tracksService;
            this.navigationService = navigationService;
            this.treeMenuService = treeMenuService;
            this.categoryId = categoryId;
            _ = LoadCategory();
            _ = LoadSubcategories();
            _ = LoadTracksFromStart();
        }

        private async Task LoadTracksFromStart()
        {
            await LoadTracksInCategory(0, tracksPerPage);
        }
        private async Task LoadCategory()
        {
            CategoryHierarchy = await categoryService.GetCategoryHierarchy(categoryId);
        }

        private async Task LoadSubcategories()
        {
            var subcategories = await categoryService.GetChildrenCategoriesAsync(categoryId);
            HasSubcategories = subcategories.Any();
            Subcategories.Clear();
            foreach (var subcategory in subcategories)
            {
                Subcategories.Add(subcategory);
            }
        }

        public async Task LoadTracksInCategory(int skip, int take, string query = "")
        {
          
            var tracks = await tracksService.GetTrackListByCategoryAsync(categoryId, skip, take);
            TotalTracks = await tracksService.GetTrackCountByCategoryAsync(categoryId);
            Pages = TotalTracks > 0 ? (TotalTracks - 1) / tracksPerPage + 1 : 0;
            HasTracks = tracks.Any();
            CategoryTracks.Clear();
            foreach(var track in tracks.ToList())
            {
                CategoryTracks.Add(track);
            }
            AvgDuration = await categoryService.GetAverageDuration(categoryId);
        }

        [RelayCommand]
        private void OpenSubcategory()
        {
            if (SelectedSubcategory == null || SelectedSubcategory.Id == null) return;
            navigationService.NavigateTo<CategoryItemsViewModel>(SelectedSubcategory.Id);
        }

        [RelayCommand]
        private void AddSubcategory()
        {
            int? parentCategoryId = categoryId;
            windowService.ShowDialog<MediaLibraryManageCategoryViewModel>(parentCategoryId, CategoryHierarchy!.Name);
            _ = LoadSubcategories();
            _ = treeMenuService.ReloadCategories();
        }

        [RelayCommand]
        private void EditItem()
        {
            if (SelectedTrack == null) return;
            windowService.ShowDialog<MediaLibraryManageTrackViewModel>(SelectedTrack.Id);
            _ = LoadTracksFromStart();
        }

        [RelayCommand]
        private void EditSubcategory()
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        private void ImportItems()
        {
            windowService.ShowDialog<MediaLibraryImportItemsViewModel>(categoryId);
            _ = LoadTracksFromStart();
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

    }
}
