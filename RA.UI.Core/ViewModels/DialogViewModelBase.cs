using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using RA.UI.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Core.ViewModels
{
    public enum RADialogResult
    {
        None = 0,
        OK = 1,
        Cancel = 2,
        Yes = 6,
        No = 7,
        Retry = 10,
        Ignore = 11,
        Abort = 3
    }
    public abstract partial class DialogViewModelBase : ViewModelBase
    {
        public String DialogName { get; protected set; } = "";

        protected readonly IWindowService windowService;
        protected DialogViewModelBase(IWindowService windowService)
        {
            this.windowService = windowService;

        }

        [RelayCommand]
        protected virtual void CancelDialog()
        {
            windowService.CloseDialog();
        }

        [RelayCommand(CanExecute = nameof(CanFinishDialog))]
        protected virtual void FinishDialog()
        {
            windowService.CloseDialog();
        }

        protected abstract bool CanFinishDialog();
    }
}
