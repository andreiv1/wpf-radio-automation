using CommunityToolkit.Mvvm.ComponentModel;
using RA.DAL;
using RA.DTO;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.MediaLibrary.ViewModels.MainContent
{
    public partial class CategoryItemsViewModel : ViewModelBase
    {
        private readonly ICategoriesService categoryService;
        private readonly ITracksService tracksService;
        private readonly int categoryId;
        public ObservableCollection<CategoryDTO> Subcategories { get; private set; } = new();

        public ObservableCollection<TrackListingDTO> CategoryTracks { get; private set; } = new();

        [ObservableProperty]
        private CategoryHierarchyDTO? categoryHierarchy;

        [ObservableProperty]
        private bool hasTracks = false;

        [ObservableProperty]
        private bool hasSubcategories = false;
        public CategoryItemsViewModel(ICategoriesService categoryService, ITracksService tracksService, int categoryId)
        {
            this.categoryService = categoryService;
            this.tracksService = tracksService;
            this.categoryId = categoryId;
            _ = LoadCategory();
            _ = LoadSubcategories();
            _ = LoadTracksInCategory();
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

        private async Task LoadTracksInCategory()
        {
            var tracks = await tracksService.GetTrackListByCategoryAsync(categoryId, 0, 100);
            HasTracks = tracks.Any();
            CategoryTracks.Clear();
            foreach(var track in tracks.ToList())
            {
                CategoryTracks.Add(track);
            }
        }

    }
}
