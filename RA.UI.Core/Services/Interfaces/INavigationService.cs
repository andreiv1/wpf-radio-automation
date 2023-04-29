using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Core.Services.Interfaces
{
    public interface INavigationService<TViewModelBase> where TViewModelBase : ViewModelBase
    {
        ViewModelBase CurrentView { get; }
        void NavigateTo<T>() where T : ViewModelBase;
        void NavigateTo<T>(object parameter) where T : ViewModelBase;
        void NavigateTo(ViewModelBase viewModel);
    }
}
