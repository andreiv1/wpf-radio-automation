using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Core.Services.Interfaces
{
    public interface IWindowService
    {
        T ShowWindow<T>() where T : ViewModelBase;
        T ShowWindow<T>(object parameter) where T : ViewModelBase;
        T ShowDialog<T>() where T : ViewModelBase;
        void CloseDialog();
        void CloseWindow(ViewModelBase viewModel);
        TViewModel ShowDialog<TViewModel>(params object[] parameters) where TViewModel : ViewModelBase;
    }
}
