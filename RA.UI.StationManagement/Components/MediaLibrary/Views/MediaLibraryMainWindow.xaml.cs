using RA.Database.Models;
using RA.DTO;
using RA.UI.Core;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.MainContent;
using RA.UI.StationManagement.Services;
using Syncfusion.UI.Xaml.TreeView;

namespace RA.UI.StationManagement.Components.MediaLibrary.Views
{
    public partial class MediaLibraryMainWindow : RAWindow
    {
        public MediaLibraryMainWindow()
        {
            InitializeComponent();
        }

        private void navigationTreeView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var vm = DataContext as MediaLibraryMainViewModel;
            var item = e.Node.Content as MenuItemModel;
            if (vm == null || item == null || item.Type != MenuItemType.Category) return;
            if(vm.TreeMenuService.SelectedItem == item)
            {
                CategoryDTO? category = item.Tag as CategoryDTO;
                if(item.Tag == null)
                {
                    vm.NavigationService.NavigateTo<CategoriesViewModel>();
                } else if(category != null)
                {
                    vm.NavigationService.NavigateTo<CategoryItemsViewModel>(category.Id);
                }
                

            }
        }
    }
}
