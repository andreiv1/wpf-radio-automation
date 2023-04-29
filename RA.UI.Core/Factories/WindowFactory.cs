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
            if (viewModelTypeToSingletonWindowTypeMap.ContainsKey(typeof(T)))
            {
                Type windowType = viewModelTypeToSingletonWindowTypeMap[typeof(T)];
                if (!typeof(Window).IsAssignableFrom(windowType))
                {
                    throw new ArgumentException("Only Windows can be created!");
                }
                Window window = (Window)serviceProvider.GetRequiredService(windowType);
                T viewModel = serviceProvider.GetRequiredService<T>();
                window.DataContext = viewModel;
                return (window, viewModel);
            }
            else if (viewModelTypeToTransientWindowTypeMap.ContainsKey(typeof(T)))
            {
                Type windowType = viewModelTypeToTransientWindowTypeMap[typeof(T)];
                if (!typeof(Window).IsAssignableFrom(windowType))
                {
                    throw new ArgumentException($"Only Windows can be created!");
                }
                Window window = (Window)ActivatorUtilities.CreateInstance(serviceProvider, windowType);
                T viewModel = serviceProvider.GetRequiredService<T>();
                window.DataContext = viewModel;
                return (window, viewModel);
            }
            else
            {
                throw new KeyNotFoundException($"The specified view model '{typeof(T).Name}' was not found in the view model to window map.");
            }
        }
        public (Window, ViewModelBase) CreateWindow<T>(object parameter) where T : ViewModelBase
        {
            return CreateWindow<T>(new object[1] { parameter });
        }
        public (Window, ViewModelBase) CreateWindow<T>(params object[] parameters) where T : ViewModelBase
        {
            if (viewModelTypeToSingletonWindowTypeMap.ContainsKey(typeof(T)))
            {
                Type windowType = viewModelTypeToSingletonWindowTypeMap[typeof(T)];
                if (!typeof(Window).IsAssignableFrom(windowType))
                {
                    throw new ArgumentException("Only Windows can be created!");
                }
                Window window = (Window)serviceProvider.GetRequiredService(windowType);
                T viewModel = (T)ActivatorUtilities.CreateInstance(serviceProvider, typeof(T), parameters);
                window.DataContext = viewModel;
                return (window, viewModel);
            }
            else if (viewModelTypeToTransientWindowTypeMap.ContainsKey(typeof(T)))
            {
                Type windowType = viewModelTypeToTransientWindowTypeMap[typeof(T)];
                if (!typeof(Window).IsAssignableFrom(windowType))
                {
                    throw new ArgumentException("Only Windows can be created!");
                }
                Window window = (Window)serviceProvider.GetRequiredService(windowType);
                T viewModel = (T)ActivatorUtilities.CreateInstance(serviceProvider, typeof(T), parameters);
                window.DataContext = viewModel;
                return (window, viewModel);
            }
            else
            {
                throw new KeyNotFoundException($"The specified view model '{typeof(T).Name}' was not found in the view model to window map.");
            }
        }

        public (Window, ViewModelBase) CreateWindow(Type type)
        {
            if (viewModelTypeToSingletonWindowTypeMap.ContainsKey(type))
            {
                Type windowType = viewModelTypeToSingletonWindowTypeMap[type];
                if (!typeof(Window).IsAssignableFrom(windowType))
                {
                    throw new ArgumentException("Only Windows can be created!");
                }
                Window window = (Window)serviceProvider.GetRequiredService(windowType);
                ViewModelBase viewModel = (ViewModelBase)serviceProvider.GetRequiredService(type);
                window.DataContext = viewModel;
                return (window, viewModel);
            }
            else if (viewModelTypeToTransientWindowTypeMap.ContainsKey(type))
            {
                Type windowType = viewModelTypeToTransientWindowTypeMap[type];
                if (!typeof(Window).IsAssignableFrom(windowType))
                {
                    throw new ArgumentException("Only Windows can be created!");
                }
                Window window = (Window)serviceProvider.GetRequiredService(windowType);
                ViewModelBase viewModel = (ViewModelBase)serviceProvider.GetRequiredService(type);
                window.DataContext = viewModel;
                window.DataContext = viewModel;
                return (window, viewModel);
            }
            else
            {
                throw new KeyNotFoundException($"The specified view model '{type.Name}' was not found in the view model to window map.");
            }
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
