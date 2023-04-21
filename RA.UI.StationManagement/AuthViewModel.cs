using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL.Interfaces;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement
{
    public partial class AuthViewModel : ViewModelBase
    {
        private readonly IWindowService windowService;
        private readonly IMessageBoxService messageBoxService;
        private readonly IUsersService usersService;
        private readonly UserStore userStore;

        public UserStore UserStore => userStore;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LogInCommand))]
        private string username = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LogInCommand))]
        private string password = string.Empty;

        public AuthViewModel(
            IWindowService windowService,
            IMessageBoxService messageBoxService,
            IUsersService usersService,
            UserStore userStore)
        {
            this.windowService = windowService;
            this.messageBoxService = messageBoxService;
            this.usersService = usersService;
            this.userStore = userStore;
        }

        [RelayCommand(CanExecute = nameof(CanLogIn))]
        private async void LogIn() {
            if (Username == null || Password == null) return;
            userStore.LoggedUser = await usersService.LogIn(Username, Password);

            bool loggedIn = userStore.LoggedUser != null;
            if (loggedIn)
            {
                //windowService.HideLastWindow(this);
                Password = string.Empty;

                if (userStore.SessionLocked)
                {
                    windowService.CloseDialog();
                }
                else
                {
                    windowService.HideLastWindow(this);
                    windowService.ShowWindow<LauncherViewModel>();
                }
            }
            else
            {
                messageBoxService.ShowError("The provided credentials are not correct.");
            }
        }

        private bool CanLogIn()
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }
    }
}
