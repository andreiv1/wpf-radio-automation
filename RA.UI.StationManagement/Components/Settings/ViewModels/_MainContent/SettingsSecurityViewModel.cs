using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL.Interfaces;
using RA.DTO;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Settings.ViewModels.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Settings.ViewModels.MainContent
{
    public partial class SettingsSecurityViewModel : ViewModelBase
    {
        private readonly IWindowService windowService;
        private readonly IMessageBoxService messageBoxService;
        private readonly IUserGroupsService userGroupsService;
        private readonly IUsersService usersService;

        public ObservableCollection<UserGroupDTO> Groups { get; private set; } = new();

        [ObservableProperty]
        private UserGroupDTO? selectedGroup;

        public ObservableCollection<UserDTO> UsersForSelectedGroup { get; private set; } = new();

        partial void OnSelectedGroupChanged(UserGroupDTO? value)
        {
            if (value != null && value.Id.HasValue)
            {
                _ = LoadUsersForGroup(value.Id.GetValueOrDefault());
            }
        }
        public SettingsSecurityViewModel(IWindowService windowService, IMessageBoxService messageBoxService,
                                         IUserGroupsService userGroupsService,
                                         IUsersService usersService)
        {
            this.windowService = windowService;
            this.messageBoxService = messageBoxService;
            this.userGroupsService = userGroupsService;
            this.usersService = usersService;
            _ = LoadGroups();
        }

        private async Task LoadGroups()
        {
            Groups.Clear();
            foreach(var g in await userGroupsService.GetGroups())
            {
                Groups.Add(g);
            }
        }

        private async Task LoadUsersForGroup(int id)
        {
            UsersForSelectedGroup.Clear();
            foreach (var u in await userGroupsService.GetUsersByGroup(id))
            {
                UsersForSelectedGroup.Add(u);
            }
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
            _ = LoadGroups();
        }

        [RelayCommand]
        private void EditUser()
        {
            windowService.ShowDialog<SettingsManageUserViewModel>();
        }

        [RelayCommand]
        private void EditGroup()
        {
            if (SelectedGroup != null)
            {
                if (SelectedGroup.IsBuiltin)
                {
                    messageBoxService.ShowError($"This group is built-in and couldn't be modified.");
                    return;
                }
                if (SelectedGroup.Id.HasValue)
                {
                    windowService.ShowDialog<SettingsManageGroupViewModel>(SelectedGroup.Id.Value);
                    _ = LoadGroups();
                }
            }
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
