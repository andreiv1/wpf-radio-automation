using CommunityToolkit.Mvvm.ComponentModel;
using RA.DAL.Interfaces;
using RA.DTO;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using System.Collections.ObjectModel;
using System.Security;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Settings.ViewModels.Security
{
    public partial class SettingsManageUserViewModel : DialogViewModelBase
    {
        private readonly int? userId;
        private readonly IMessageBoxService messageBoxService;
        private readonly IUsersService usersService;
        private readonly IUserGroupsService userGroupsService;

        public ObservableCollection<UserGroupDTO> Groups { get; private set; } = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FinishDialogCommand))]
        private string username = "";

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FinishDialogCommand))]
        private string fullName = "";

        private string password = "";
        public string Password
        {
            get => password; set
            {
                password = value;
                FinishDialogCommand.NotifyCanExecuteChanged();
            }
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FinishDialogCommand))]
        private UserGroupDTO? selectedGroup;
        public SettingsManageUserViewModel(IWindowService windowService, IMessageBoxService messageBoxService,
            IUsersService usersService,
            IUserGroupsService userGroupsService) : base(windowService)
        {
            this.messageBoxService = messageBoxService;
            this.usersService = usersService;
            this.userGroupsService = userGroupsService;
            _ = LoadGroups();
        }
        public SettingsManageUserViewModel(IWindowService windowService,
                                          IMessageBoxService messageBoxService,
                                          IUserGroupsService userGroupsService,
                                          IUsersService usersService,
                                          int userId) 
            : this(windowService, messageBoxService, usersService, userGroupsService)
        {
            this.userId = userId;
        }

        private async Task LoadGroups()
        {
            Groups.Clear();
            foreach (var g in await userGroupsService.GetGroups())
            {
                Groups.Add(g);
            }
        }

        private async Task<bool> AddUser()
        {
            if (SelectedGroup != null && SelectedGroup.Id != null)
            {
                var dto = new UserDTO()
                {
                    Username = Username,
                    FullName = FullName,
                    Password = Password.ToString(),
                    GroupId = SelectedGroup.Id.Value,
                };

                bool result = await usersService.AddUser(dto);
                return result;
            }
            return false;
        }

        private async Task UpdateUser()
        {
            if (SelectedGroup != null && SelectedGroup.Id != null && userId != null)
            {
                var dto = new UserDTO()
                {
                    Id = userId.Value,
                    Username = Username,
                    FullName = FullName,
                    Password = Password.ToString(),
                    GroupId = SelectedGroup.Id.Value,
                };

                throw new System.Exception();
            }
        }
        protected async override void FinishDialog()
        {
            if(userId == null)
            {
                var result = await Task.Run(AddUser);
                if (!result)
                {
                    messageBoxService.ShowWarning($"Username {Username} is already in use!");
                    return;
                }

            } else
            {
                await Task.Run(UpdateUser);
            }
            base.FinishDialog();
        }
        protected override bool CanFinishDialog()
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(FullName) && SelectedGroup != null;
        }
    }
}
