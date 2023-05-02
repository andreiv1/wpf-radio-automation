using CommunityToolkit.Mvvm.ComponentModel;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.MainContent;
using RA.UI.StationManagement.Services;
using System.Linq;

namespace RA.UI.StationManagement.Components.MediaLibrary.ViewModels
{
    public partial class MediaLibraryMainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private INavigationService<MediaLibraryMainViewModel> navigationService;
        [ObservableProperty]
        private MediaLibraryTreeMenuService treeMenuService;

        public MediaLibraryMainViewModel(INavigationService<MediaLibraryMainViewModel> navigationService,
                                         MediaLibraryTreeMenuService treeMenuService)
        {
            this.navigationService = navigationService;
            this.treeMenuService = treeMenuService;
            navigationService.NavigateTo<AllMediaItemsViewModel>();
            this.treeMenuService.SelectedItem = treeMenuService.MenuItems.FirstOrDefault();
        }
    }
}
