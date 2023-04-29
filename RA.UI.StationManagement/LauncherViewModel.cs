using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.MainContent;
using RA.UI.StationManagement.Components.Planner.ViewModels;
using RA.UI.StationManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var result = windowService.ShowWindow<MediaLibraryMainViewModel>();
        }

        [RelayCommand]
        private void OpenPlannerComponent()
        {
            var result = windowService.ShowWindow<PlannerMainViewModel>();
        }
    }
}
