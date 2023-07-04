using CommunityToolkit.Mvvm.Input;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels;
using RA.UI.StationManagement.Components.Reports.ViewModels;
using RA.UI.StationManagement.Components.Settings.ViewModels;
using System.Windows;

namespace RA.UI.StationManagement
{
    public partial class LauncherViewModel : ViewModelBase
    {
        private readonly IWindowService windowService;
        public LauncherViewModel(IWindowService windowService)
        {
            this.windowService = windowService;
        }

        [RelayCommand]
        private void OpenMediaLibraryComponent()
        {
            windowService.ShowWindow<MediaLibraryMainViewModel>();
        }

        [RelayCommand]
        private void OpenPlannerComponent()
        {
            windowService.ShowWindow<PlannerMainViewModel>();
        }


        [RelayCommand]
        private void OpenReportsComponent()
        {
            windowService.ShowWindow<ReportsMainViewModel>();
        }

        [RelayCommand]
        private void OpenSettingsComponent()
        {
            windowService.ShowWindow<SettingsMainViewModel>();
        }

        [RelayCommand]
        private void LockSession()
        {
            windowService.ShowDialog<AuthViewModel>();
        }

        [RelayCommand]
        private void ExitApp()
        {
            Application.Current.Shutdown();
        }
    }
}
