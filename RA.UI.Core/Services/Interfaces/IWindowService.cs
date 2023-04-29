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
        T ShowDialog<T>(object parameter) where T : ViewModelBase;

        T ShowDialog<T>(object param1, object param2) where T : ViewModelBase;
        void CloseDialog();
        void CloseWindow(ViewModelBase viewModel);
    }
}
