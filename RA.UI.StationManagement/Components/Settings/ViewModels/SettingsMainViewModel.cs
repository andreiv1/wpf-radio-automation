using CommunityToolkit.Mvvm.ComponentModel;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Settings.ViewModels.MainContent;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace RA.UI.StationManagement.Components.Settings.ViewModels
{
    public partial class SettingsMenuModel : ObservableObject
    {
        [ObservableProperty]
        private string header;

        public Type ViewModel { get; set; }
        public ImageSource Icon { get; }

        public SettingsMenuModel(string header, Type viewModel, ImageSource icon)
        {
            this.header = header;
            ViewModel = viewModel;
            Icon = icon;
        }
    }
    public partial class SettingsMainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private INavigationService<SettingsMainViewModel> navigationService;

        public ObservableCollection<SettingsMenuModel> SettingsMenu { get; set; } = new()
        {
            new SettingsMenuModel("General", typeof(SettingsGeneralViewModel), (ImageSource)Application.Current.Resources["AdministrativeToolsIcon"]),
            //new SettingsMenuModel("Database", typeof(SettingsDatabaseViewModel), (ImageSource)Application.Current.Resources["DatabaseAdministratorIcon"]),
            new SettingsMenuModel("Security", typeof(SettingsSecurityViewModel), (ImageSource)Application.Current.Resources["SecurityConfigurationIcon"]),
            new SettingsMenuModel("About", typeof(SettingsAboutViewModel), (ImageSource)Application.Current.Resources["UserManualIcon"]),
        };

        [ObservableProperty]
        private SettingsMenuModel selectedSettings;

        partial void OnSelectedSettingsChanged(SettingsMenuModel value)
        {
            if(value.ViewModel == typeof(SettingsGeneralViewModel))
            {
                NavigationService.NavigateTo<SettingsGeneralViewModel>();
            } else if(value.ViewModel == typeof(SettingsDatabaseViewModel))
            {
                NavigationService.NavigateTo<SettingsDatabaseViewModel>();
            } else if(value.ViewModel == typeof(SettingsSecurityViewModel)){
                NavigationService.NavigateTo<SettingsSecurityViewModel>();
            } else if(value.ViewModel == typeof(SettingsAboutViewModel))
            {
                NavigationService.NavigateTo<SettingsAboutViewModel>();
            }
            
        }
        public SettingsMainViewModel(INavigationService<SettingsMainViewModel> navigationService)
        {
            this.navigationService = navigationService;
            SelectedSettings = SettingsMenu.First();
        }
    }
}
