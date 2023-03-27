using Microsoft.Extensions.DependencyInjection;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RA.UI.Core.Factories
{
    public interface IWindowFactory
    {
        public (Window, ViewModelBase) CreateWindow<T>() where T : ViewModelBase;
        public (Window, ViewModelBase) CreateWindow(Type type);
        public (Window, ViewModelBase) CreateWindow<T>(object parameter) where T : ViewModelBase;
        public (Window, ViewModelBase) CreateWindow<T>(object[] parameters) where T : ViewModelBase;

        public Window? GetWindow(ViewModelBase viewModel);
    }
    public class WindowFactory : IWindowFactory
    {
        private readonly Dictionary<Type, Type> viewModelTypeToSingletonWindowTypeMap;
        private readonly Dictionary<Type, Type> viewModelTypeToTransientWindowTypeMap;
        private readonly IServiceProvider serviceProvider;

        public WindowFactory(
            Dictionary<Type, Type> viewModelTypeToSingletonWindowTypeMap,
            Dictionary<Type, Type> viewModelTypeToTransientWindowTypeMap,
            IServiceProvider serviceProvider)
        {
            this.viewModelTypeToSingletonWindowTypeMap = viewModelTypeToSingletonWindowTypeMap;
            this.viewModelTypeToTransientWindowTypeMap = viewModelTypeToTransientWindowTypeMap;
            this.serviceProvider = serviceProvider;
        }

        public (Window, ViewModelBase) CreateWindow<T>() where T : ViewModelBase
        {
            return CreateWindow(typeof(T));
        }

        public (Window, ViewModelBase) CreateWindow<T>(object parameter) where T : ViewModelBase
        {
            return CreateWindow(typeof(T), new object[] { parameter });
        }

        public (Window, ViewModelBase) CreateWindow<T>(params object[] parameters) where T : ViewModelBase
        {
            return CreateWindow(typeof(T), parameters);
        }

        public (Window, ViewModelBase) CreateWindow(Type type)
        {
            return CreateWindow(type, new object[0]);
        }

        private (Window, ViewModelBase) CreateWindow(Type type, object[] parameters)
        {
            Dictionary<Type, Type> windowTypeMap = viewModelTypeToSingletonWindowTypeMap.ContainsKey(type)
                ? viewModelTypeToSingletonWindowTypeMap
                : viewModelTypeToTransientWindowTypeMap;

            if (!windowTypeMap.ContainsKey(type))
            {
                throw new KeyNotFoundException($"The specified view model '{type.Name}' was not found in the view model to window map.");
            }

            Type windowType = windowTypeMap[type];
            if (!typeof(Window).IsAssignableFrom(windowType))
            {
                throw new ArgumentException("Only Windows can be created!");
            }

            using var serviceScope = serviceProvider.CreateScope();
            Window window = (Window)serviceScope.ServiceProvider.GetRequiredService(windowType);
            ViewModelBase viewModel = (ViewModelBase)ActivatorUtilities.CreateInstance(serviceScope.ServiceProvider, type, parameters);
            window.DataContext = viewModel;

            return (window, viewModel);
        }

        public Window? GetWindow(ViewModelBase viewModel)
        {
            var focusedElement = Keyboard.FocusedElement as FrameworkElement;
            if (focusedElement != null)
            {
                Window window = Window.GetWindow(focusedElement);
                if (window.DataContext == viewModel)
                {
                    return window;
                }
            }
            return null;
        }
    }
}
