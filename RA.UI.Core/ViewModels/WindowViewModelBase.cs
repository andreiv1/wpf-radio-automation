using CommunityToolkit.Mvvm.Input;
using RA.UI.Core.Services.Interfaces;

namespace RA.UI.Core.ViewModels
{
    public partial class WindowViewModelBase : ViewModelBase
    {
        protected readonly IWindowService windowService;

        public WindowViewModelBase(IWindowService windowService)
        {
            this.windowService = windowService;
        }

        [RelayCommand]
        private void CloseWindow()
        {
            windowService.CloseWindow(this);
        }
    }
}
