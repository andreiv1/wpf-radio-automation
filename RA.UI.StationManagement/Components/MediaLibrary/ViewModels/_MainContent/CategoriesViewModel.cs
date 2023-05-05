using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.Database.Models;
using RA.DTO;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.Categories;
using RA.UI.StationManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.MediaLibrary.ViewModels.MainContent
{
    public partial class CategoriesViewModel : ViewModelBase
    {
        private readonly IWindowService windowService;
        private readonly ICategoriesService categoriesService;
        private readonly INavigationService<MediaLibraryMainViewModel> navigationService;
        private readonly MediaLibraryTreeMenuService treeMenuService;

        public ObservableCollection<CategoryDTO> Categories { get; set; } = new();

        [ObservableProperty]
        private CategoryDTO? selectedCategory;

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
                    //DebugHelper.WriteLine(this, $"Performing search query: {value}");
                    //_ = LoadTracks(0, tracksPerPage, value);
                }
            });
        }

        public CategoriesViewModel(IWindowService windowService,
                                   ICategoriesService categoriesService,
                                   INavigationService<MediaLibraryMainViewModel> navigationService,
                                   MediaLibraryTreeMenuService treeMenuService)
        {
            this.windowService = windowService;
            this.categoriesService = categoriesService;
            this.navigationService = navigationService;
            this.treeMenuService = treeMenuService;
          
            _ = LoadCategories();
        }

        private async Task LoadCategories()
        {
            Categories.Clear();
            var categories = await categoriesService.GetRootCategoriesAsync();
            foreach(var category in categories) { 
                Categories.Add(category);
            }
       
        }

        //Commands
        [RelayCommand]
        private void AddCategory()
        {
            var vm = windowService.ShowDialog<MediaLibraryManageCategoryViewModel>();
            _ = LoadCategories();
            _ = treeMenuService.ReloadCategories();
        }

        [RelayCommand]
        private void EditCategory()
        {
            if(SelectedCategory == null || SelectedCategory.Id == null) return;
            windowService.ShowDialog<MediaLibraryManageCategoryViewModel>(SelectedCategory.Id);
        }

        [RelayCommand(CanExecute = nameof(CanOpenCategory))]
        
        private void OpenCategory()
        {
            if (SelectedCategory == null || SelectedCategory.Id == null) return;
            navigationService.NavigateTo<CategoryItemsViewModel>(SelectedCategory.Id);
        }

        private bool CanOpenCategory()
        {
            //if (SelectedCategory == null || SelectedCategory.Id == null) return false;
            //int categoryId = SelectedCategory.Id.Value;
            //return !categoriesService.HasCategoryChildren(categoryId).Result;
            return true;
        }


        [RelayCommand]
        private void DeleteItem()
        {
            //throw new NotImplementedException();
        }
        

       

    }
}
