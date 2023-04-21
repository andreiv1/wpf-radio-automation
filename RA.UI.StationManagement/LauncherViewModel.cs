using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.Database.Models;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels;
using RA.UI.StationManagement.Components.Reports.ViewModels;
using RA.UI.StationManagement.Components.Settings.ViewModels;
using RA.UI.StationManagement.Stores;
using System.Windows;

namespace RA.UI.StationManagement
{
    public partial class LauncherViewModel : ViewModelBase
    {
        private readonly IWindowService windowService;
        private readonly UserStore userStore;

        [ObservableProperty]
        private string? displayName;

        partial void OnDisplayNameChanged(string? value)
        {
            OpenMediaLibraryComponentCommand.NotifyCanExecuteChanged();
            OpenPlannerComponentCommand.NotifyCanExecuteChanged();
            OpenReportsComponentCommand.NotifyCanExecuteChanged();
            OpenSettingsComponentCommand.NotifyCanExecuteChanged();
        }

        public LauncherViewModel(IWindowService windowService,
                                 UserStore userStore)
        {
            this.windowService = windowService;
            this.userStore = userStore;
            DisplayName = userStore.LoggedUser?.FullName;
            
        }

        [RelayCommand(CanExecute = nameof(CanOpenMediaLibrary))]
        private void OpenMediaLibraryComponent()
        {
            windowService.ShowWindow<MediaLibraryMainViewModel>();
        }

        private bool CanOpenMediaLibrary()
        {
            return userStore.CheckPermissions(UserRuleType.ACCESS_MEDIA_LIBRARY);
        }

        [RelayCommand(CanExecute = nameof(CanOpenPlanner))]
        private void OpenPlannerComponent()
        {
            windowService.ShowWindow<PlannerMainViewModel>();
        }

        private bool CanOpenPlanner()
        {
          return userStore.CheckPermissions(UserRuleType.ACCESS_PLANNER);
        }

        [RelayCommand(CanExecute = nameof(CanOpenReports))]
        private void OpenReportsComponent()
        {
            windowService.ShowWindow<ReportsMainViewModel>();
        }

        private bool CanOpenReports()
        {
            return userStore.CheckPermissions(UserRuleType.ACCESS_REPORTS);
        }

        [RelayCommand(CanExecute = nameof(CanOpenSettings))]
        private void OpenSettingsComponent()
        {
            windowService.ShowWindow<SettingsMainViewModel>();
        }

        private bool CanOpenSettings()
        {
            return userStore.CheckPermissions(UserRuleType.ACCESS_SETTINGS);
        }

        [RelayCommand]
        private void LockSession()
        {
            foreach(var window in Application.Current.Windows)
            {
                if (window is not LauncherWindow)
                {
                    var windowToClose = window as Window;
                    windowToClose?.Close();
                }
            }
            userStore.LoggedUser = null;
            userStore.SessionLocked = true;
            windowService.ShowDialog<AuthViewModel>();
            if (!userStore.SessionLocked)
            {
                windowService.CloseLastHiddenWindow();
            }
            DisplayName = userStore.LoggedUser?.FullName;
        }

        [RelayCommand]
        private void ExitApp()
        {
            Application.Current.Shutdown();
        }
    }
}
