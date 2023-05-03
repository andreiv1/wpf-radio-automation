using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DTO;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.Categories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.MediaLibrary.ViewModels.MainContent
{
    public partial class CategoriesViewModel : ViewModelBase
    {
        private readonly IWindowService windowService;
        private readonly ICategoriesService categoriesService;

        public ObservableCollection<CategoryDTO> Categories { get; set; } = new();

        public CategoriesViewModel(IWindowService windowService, ICategoriesService categoriesService)
        {
            this.windowService = windowService;
            this.categoriesService = categoriesService;
            _ = LoadCategories();
        }

        private async Task LoadCategories()
        {
            IsMainDataLoading = true;
            var categories = await categoriesService.GetRootCategoriesAsync();
            foreach(var category in categories) { 
                Categories.Add(category);
            }
            IsMainDataLoading = false;
        }

        #region Commands
        [RelayCommand]
        private void AddCategory()
        {
            windowService.ShowDialog<MediaLibraryManageCategoryViewModel>();
        }
        #endregion
    }
}
