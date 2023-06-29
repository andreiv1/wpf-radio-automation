using CommunityToolkit.Mvvm.Input;
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

        public AuthViewModel(IWindowService windowService)
        {
            this.windowService = windowService;
        }

        [RelayCommand]
        private void LogIn() { 
            windowService.CloseWindow(this);
            windowService.ShowWindow<LauncherViewModel>();
        }
    }
}
