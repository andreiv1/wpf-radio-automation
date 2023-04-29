using RA.UI.Core.Factories;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RA.UI.Core.Services
{
    public class WindowService : IWindowService
    {
        private readonly IWindowFactory windowFactory;
        private readonly Stack<Window> dialogStack = new Stack<Window>();

        public WindowService(IWindowFactory windowFactory)
        {
            this.windowFactory = windowFactory;
        }

        private TViewModel ShowWindowInternal<TViewModel>(Window window, ViewModelBase viewModel) where TViewModel : ViewModelBase
        {
            window.DataContext = viewModel;
            window.Show();
            return (TViewModel)viewModel;
        }

        private TViewModel ShowDialogInternal<TViewModel>(Window window, ViewModelBase viewModel) where TViewModel : ViewModelBase
        {
            dialogStack.Push(window);
            window.Closed += Window_Closed!;
            window.DataContext = viewModel;
            window.ShowDialog();
            return (TViewModel)viewModel;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (dialogStack.Count > 0)
            {
                Window window = (Window)sender;
                dialogStack.Pop();
            }
        }

        public TViewModel ShowWindow<TViewModel>() where TViewModel : ViewModelBase
        {
            (Window window, ViewModelBase viewModel) = windowFactory.CreateWindow<TViewModel>();
            return ShowWindowInternal<TViewModel>(window, viewModel);
        }

        public TViewModel ShowWindow<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            (Window window, ViewModelBase viewModel) = windowFactory.CreateWindow<TViewModel>(parameter);
            return ShowWindowInternal<TViewModel>(window, viewModel);
        }

        public TViewModel ShowDialog<TViewModel>() where TViewModel : ViewModelBase
        {
            (Window window, ViewModelBase viewModel) = windowFactory.CreateWindow<TViewModel>();
            return ShowDialogInternal<TViewModel>(window, viewModel);
        }

        public TViewModel ShowDialog<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            (Window window, ViewModelBase viewModel) = windowFactory.CreateWindow<TViewModel>(parameter);
            return ShowDialogInternal<TViewModel>(window, viewModel);
        }

        public TViewModel ShowDialog<TViewModel>(object param1, object param2) where TViewModel : ViewModelBase
        {
            (Window window, ViewModelBase viewModel) = windowFactory.CreateWindow<TViewModel>(new object[] { param1, param2 });
            return ShowDialogInternal<TViewModel>(window, viewModel);
        }

        public void CloseDialog()
        {
            if (dialogStack.Count > 0)
            {
                Window dialog = dialogStack.Peek();
                dialog.Closed -= Window_Closed!; // remove the event handler if it was previously set
                dialogStack.Pop();
                dialog.Close();
            }
        }

        public void CloseWindow(ViewModelBase viewModel)
        {
            Window? window = windowFactory.GetWindow(viewModel);
            if (window is not null)
            {
                window.Close();
            }
        }
    }

}
