using Microsoft.Extensions.DependencyInjection;
using RA.Logic.AudioPlayer;
using RA.Logic.AudioPlayer.Interfaces;
using RA.UI.Core.Factories;
using RA.UI.Playout.ViewModels;
using RA.UI.Playout.ViewModels.Controls;
using RA.UI.Playout.Views;
using RA.UI.Playout.Views.Components;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RA.UI.Playout
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider serviceProvider;

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTc3ODYwNUAzMjMxMmUzMTJlMzMzNWc2NlM4V1ZTSm9xU0htNmhzSE1GZDBicktpMWZKY1VoaHdMb3pSaWhmclU9");

            IServiceCollection services = new ServiceCollection();
            ConfigureSingletonServices(services);
            ConfigureTransientServices(services);
            ConfigureViews(services);

            services.AddSingleton(services);
            serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            SplashScreenWindow splashScreen = new SplashScreenWindow();
            splashScreen.Show();

            //Eager load
            var loadPlaybackService = Task.Run(() =>
            {
                _ = serviceProvider.GetRequiredService<IAudioPlayer>();
                _ = serviceProvider.GetRequiredService<IPlaybackQueue>();
            });


            var loadMainViewModel = Task.Run(() =>
            {
                _ = serviceProvider.GetRequiredService<MainViewModel>();
            });

            Task.WhenAll(loadPlaybackService, loadMainViewModel).ContinueWith(t =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
                    mainWindow.Show();
                    splashScreen.Close();
                });
            });

            base.OnStartup(e);
        }

        private void ConfigureSingletonServices(IServiceCollection services)
        {
            services.AddSingleton<IAudioPlayer, AudioPlayer>();
            services.AddSingleton<IPlaybackQueue, PlaybackQueue>();
        }
        private void ConfigureTransientServices(IServiceCollection services)
        {
        }

        private void ConfigureSingletonViews(IServiceCollection services, Dictionary<Type, Type> viewModelToSingletonWindowMap)
        {
            viewModelToSingletonWindowMap.Add(typeof(MainViewModel), typeof(MainWindow));
            viewModelToSingletonWindowMap.Add(typeof(NowPlayingViewModel), typeof(NowPlayingView));
            viewModelToSingletonWindowMap.Add(typeof(PlaylistViewModel), typeof(PlaylistView));
            viewModelToSingletonWindowMap.Add(typeof(MediaItemsViewModel), typeof(MediaItemsView));

            foreach (var item in viewModelToSingletonWindowMap)
            {
                Type viewModelType = item.Key;
                Type windowType = item.Value;

                services.AddSingleton(viewModelType);

                //TODO: Bug #106
                //Do not use as TRANSIENT!!! SINGLETON!!!!!
                services.AddTransient(windowType, provider =>
                {
                    //Use WindowFactory
                    var window = (Window)Activator.CreateInstance(windowType);
                    var viewModel = provider.GetRequiredService(viewModelType);
                    window.DataContext = viewModel;
                    return window;
                });
            }
        }
        private void ConfigureTransientViews(IServiceCollection services, Dictionary<Type, Type> viewModelToTransientWindowMap)
        {
            //viewModelToTransientWindowMap.Add();


            foreach (var item in viewModelToTransientWindowMap)
            {
                services.AddTransient(item.Key);
                services.AddTransient(item.Value);
            }
        }

        private void ConfigureViews(IServiceCollection services)
        {
            // Singleton windows
            Dictionary<Type, Type> viewModelToSingletonWindowMap = new();
            ConfigureSingletonViews(services, viewModelToSingletonWindowMap);

            // Transient windows
            Dictionary<Type, Type> viewModelToTransientWindowMap = new();
            ConfigureTransientViews(services, viewModelToTransientWindowMap);

            // Window factory
            services.AddSingleton<IWindowFactory>(provider =>
                new WindowFactory(
                    viewModelToSingletonWindowMap,
                    viewModelToTransientWindowMap,
                    serviceProvider
                )
            );
        }
    }
}
