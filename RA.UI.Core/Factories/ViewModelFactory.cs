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
            using var serviceScope = serviceProvider.CreateScope();
            var viewModel = serviceScope.ServiceProvider.GetService(typeof(TViewModel));
            return viewModel != null ? (TViewModel)viewModel : throw new ArgumentNullException(nameof(viewModel));
        }

        public TViewModel CreateViewModel<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            using var serviceScope = serviceProvider.CreateScope();
            var viewModel = ActivatorUtilities.CreateInstance(serviceScope.ServiceProvider, typeof(TViewModel), new object[] { parameter });
            return viewModel != null ? (TViewModel)viewModel : throw new ArgumentNullException(nameof(viewModel));
        }

        public ViewModelBase CreateViewModel(Type viewModelType)
        {
            using var serviceScope = serviceProvider.CreateScope();
            var viewModel = serviceScope.ServiceProvider.GetService(viewModelType);
            return viewModel != null ? (ViewModelBase)viewModel : throw new ArgumentNullException(nameof(viewModel));
        }

        public ViewModelBase CreateViewModel(Type viewModelType, object parameter)
        {
            using var serviceScope = serviceProvider.CreateScope();
            var viewModel = ActivatorUtilities.CreateInstance(serviceScope.ServiceProvider, viewModelType, new object[] {parameter});
            return viewModel != null ? (ViewModelBase)viewModel : throw new ArgumentNullException(nameof(viewModel));
        }
    }
}
