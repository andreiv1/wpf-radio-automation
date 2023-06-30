using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL.Interfaces;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
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

        private static bool isFirstOpened = true;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LogInCommand))]
        private string username = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LogInCommand))]
        private string password = string.Empty;

        public AuthViewModel(
            IWindowService windowService,
            IMessageBoxService messageBoxService,
            IUsersService usersService)
        {
            this.windowService = windowService;
            this.messageBoxService = messageBoxService;
            this.usersService = usersService;
        }

        [RelayCommand(CanExecute = nameof(CanLogIn))]
        private async void LogIn() {
            if (Username == null || Password == null) return;
            var canThisUserLogin = await usersService.CanUserLogIn(Username, Password); 
            
            if(canThisUserLogin)
            {
                windowService.CloseWindow(this);
                if(isFirstOpened)
                {
                    isFirstOpened = false;
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
