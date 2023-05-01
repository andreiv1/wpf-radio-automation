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
    public partial class CategoriesViewModel : ViewModelBase
    {
        private readonly ICategoriesService categoriesService;

        public ObservableCollection<CategoryDto> Categories { get; set; } = new();

        public CategoriesViewModel(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
            _ = LoadCategories();
        }

        private async Task LoadCategories()
        {
            var categories = await categoriesService.GetRootCategoriesAsync();
            foreach(var category in categories) { 
                Categories.Add(category);
            }
        }
    }
}
