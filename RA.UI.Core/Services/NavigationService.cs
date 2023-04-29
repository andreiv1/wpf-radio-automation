using CommunityToolkit.Mvvm.ComponentModel;
using RA.UI.Core.Factories;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Core.Services
{
    public class NavigationService<TBaseViewModel> : ObservableObject, INavigationService<TBaseViewModel>
        where TBaseViewModel : ViewModelBase
    {
        private ViewModelBase? currentView;
        private readonly IViewModelFactory viewModelFactory;

        public ViewModelBase CurrentView
        {
            get => currentView;
            private set
            {
                //TODO: dispose old view model to avoid memory leaks
                currentView?.Dispose();
                currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public NavigationService(IViewModelFactory viewModelFactory) => this.viewModelFactory = viewModelFactory;

        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            ViewModelBase viewModelBase = viewModelFactory.CreateViewModel<TViewModel>();
            CurrentView = viewModelBase;
        }

        public void NavigateTo<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            ViewModelBase viewModelBase = viewModelFactory.CreateViewModel<TViewModel>(parameter);
            CurrentView = viewModelBase;
        }

        public void NavigateTo(ViewModelBase viewModel)
        {
            CurrentView = viewModel;
        }

        
    }
}
