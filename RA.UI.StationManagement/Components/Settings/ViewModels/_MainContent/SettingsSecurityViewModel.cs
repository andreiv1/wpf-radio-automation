using CommunityToolkit.Mvvm.Input;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Settings.ViewModels.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Settings.ViewModels.MainContent
{
    public partial class SettingsSecurityViewModel : ViewModelBase
    {
        private readonly IWindowService windowService;

        public SettingsSecurityViewModel(IWindowService windowService)
        {
            this.windowService = windowService;
        }

        [RelayCommand]
        private void AddUser()
        {
            windowService.ShowDialog<SettingsManageUserViewModel>();
        }

        [RelayCommand]
        private void AddGroup()
        {
            windowService.ShowDialog<SettingsManageGroupViewModel>();
        }

        [RelayCommand]
        private void EditUser()
        {
            windowService.ShowDialog<SettingsManageUserViewModel>();
        }

        [RelayCommand]
        private void EditGroup()
        {
            windowService.ShowDialog<SettingsManageGroupViewModel>();
        }

        [RelayCommand]
        private void RemoveUser()
        {

        }

        [RelayCommand]
        private void RemoveGroup()
        {

        }
    }
}
