using CommunityToolkit.Mvvm.ComponentModel;
using RA.DAL;
using RA.DTO;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.MediaLibrary.ViewModels.Categories
{
    public partial class MediaLibraryManageCategoryViewModel : DialogViewModelBase
    {
        [ObservableProperty]
        private CategoryDTO category;

        private readonly ICategoriesService categoriesService;

        public MediaLibraryManageCategoryViewModel(IWindowService windowService, ICategoriesService categoriesService) : base(windowService)
        {
            this.categoriesService = categoriesService;
            Category = new();
        }

        public MediaLibraryManageCategoryViewModel(IWindowService windowService, ICategoriesService categoriesService, int categoryId) : base(windowService)
        {
            this.categoriesService = categoriesService;
            _ = LoadCategory(categoryId);
        }

        private async Task LoadCategory(int categoryId)
        {
            Category = await categoriesService.GetCategory(categoryId);
        }
        protected override bool CanFinishDialog()
        {
            return true;
        }
    }
}
