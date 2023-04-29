using Microsoft.Extensions.DependencyInjection;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Core.Factories
{
    public interface IViewModelFactory
    {
        public T CreateViewModel<T>() where T : ViewModelBase;

        public T CreateViewModel<T>(object parameter) where T : ViewModelBase;

        public ViewModelBase CreateViewModel(Type viewModelType);
        public ViewModelBase CreateViewModel(Type viewModelType, object parameter);
    }

    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ViewModelFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public TViewModel CreateViewModel<TViewModel>() where TViewModel : ViewModelBase
        {
            var viewModel = serviceProvider.GetService(typeof(TViewModel));
            return viewModel != null ? (TViewModel)viewModel : throw new ArgumentNullException(nameof(viewModel));
        }

        public TViewModel CreateViewModel<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            var viewModel = ActivatorUtilities.CreateInstance(serviceProvider, typeof(TViewModel), new object[] { parameter });
            return viewModel != null ? (TViewModel)viewModel : throw new ArgumentNullException(nameof(viewModel));
        }

        public ViewModelBase CreateViewModel(Type viewModelType)
        {
            var viewModel = serviceProvider.GetService(viewModelType);
            return viewModel != null ? (ViewModelBase)viewModel : throw new ArgumentNullException(nameof(viewModel));
        }

        public ViewModelBase CreateViewModel(Type viewModelType, object parameter)
        {
            var viewModel = ActivatorUtilities.CreateInstance(serviceProvider, viewModelType, new object[] {parameter});
            return viewModel != null ? (ViewModelBase)viewModel : throw new ArgumentNullException(nameof(viewModel));
        }
    }
}
