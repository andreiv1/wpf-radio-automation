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
        private readonly int categoryId;
        public ObservableCollection<CategoryDto> Subcategories { get; set; } = new();

        public ObservableCollection<TrackListDto> CategoryTracks { get; set; } = new();

        [ObservableProperty]
        private CategoryHierarchyDto? categoryHierarchy;

        [ObservableProperty]
        private bool hasItems = false;

        [ObservableProperty]
        private bool hasSubcategories = false;
        public CategoryItemsViewModel(ICategoriesService categoryService, int categoryId)
        {
            this.categoryService = categoryService;
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
            Subcategories.Clear();
            foreach (var subcategory in subcategories)
            {
                Subcategories.Add(subcategory);
            }
        }

        private async Task LoadTracksInCategory()
        {
            var tracks = await categoryService.GetTrackListByCategoryAsync(categoryId, 0, 100);
            CategoryTracks.Clear();
            foreach(var track in tracks)
            {
                CategoryTracks.Add(track);
            }
        }

    }
}
