using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySqlConnector;
using RA.DAL;
using RA.Database;
using RA.Logic.AudioPlayer;
using RA.Logic.AudioPlayer.Interfaces;
using RA.UI.Core.Factories;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.Themes;
using RA.UI.Playout.ViewModels;
using RA.UI.Playout.ViewModels.Components;
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
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTc3ODYwNUAzMjMxMmUzMTJlMzMzNWc2NlM4V1ZTSm9xU0htNmhzSE1GZDBicktpMWZKY1VoaHdMb3pSaWhmclU9");


            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContextFactory<AppDbContext>(options =>
                    {
                        String connString = "server=localhost;Port=3306;database=ratest;user=root;password=";
                        options.UseMySql(connString, ServerVersion.AutoDetect(connString))
                            .EnableSensitiveDataLogging(true);
                    });

                    services.AddSingleton<IWindowService, WindowService>();
                    services.AddSingleton<IViewModelFactory, ViewModelFactory>();
                    services.AddSingleton<IMessageBoxService, MessageBoxService>();
                    services.AddSingleton<IDispatcherService, WpfDispatcherService>();

                    #region Logic services
                    services.AddSingleton<IAudioPlayer, AudioPlayer>();
                    services.AddSingleton<IPlaybackQueue, PlaybackQueue>();
                    #endregion

                    #region DAL services
                    services.AddTransient<IPlaylistsService,PlaylistsService>();

                    #endregion
                    #region Register window factory
                    Dictionary<Type, Type> viewModelToSingletonWindowMap = new();
                    Dictionary<Type, Type> viewModelToTransientWindowMap = new();
                    services.AddSingleton<IWindowFactory>(provider =>
                        new WindowFactory(
                            viewModelToSingletonWindowMap,
                            viewModelToTransientWindowMap,
                            AppHost!.Services
                        )
                    );

                    #endregion

                    #region Singleton viewModel <=> View
                    viewModelToSingletonWindowMap.Add(typeof(MainViewModel), typeof(MainWindow));

                    viewModelToSingletonWindowMap.Add(typeof(MediaItemsViewModel), typeof(MediaItemsView));
                    viewModelToSingletonWindowMap.Add(typeof(NowPlayingViewModel), typeof(NowPlayingView));
                    viewModelToSingletonWindowMap.Add(typeof(PlaylistViewModel), typeof(PlaylistView));
  
                    //Register the viewmodel&view
                    foreach (KeyValuePair<Type, Type> vmAndView in viewModelToSingletonWindowMap)
                    {
                        //Key: ViewModel, //Value: View
                        //Register viewmodel
                        services.AddSingleton(vmAndView.Key);
                        //Register view
                        services.AddTransient(vmAndView.Value, provider =>
                        {
                            var viewModel = provider.GetRequiredService(vmAndView.Key);
                            var window = (Window)Activator.CreateInstance(vmAndView.Value)!;
                            window.DataContext = viewModel;
                            return window;
                        });
                    }
                    #endregion

                    #region Transient ViewModel <=> View
                    foreach (KeyValuePair<Type, Type> vmAndView in viewModelToTransientWindowMap)
                    {
                        services.AddTransient(vmAndView.Key);
                        services.AddTransient(vmAndView.Value);
                    }
                    #endregion




                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();
            StartApp();
            base.OnStartup(e);
        }

        private void StartApp()
        {
            var windowService = AppHost!.Services.GetRequiredService<IWindowService>();
            var dispatcherService = AppHost!.Services.GetRequiredService<IDispatcherService>();
            ThemeManager.SetTheme(ThemeType.Dark);
            SplashScreenWindow splashScreen = new SplashScreenWindow();
            splashScreen.Show();

            var testDatabaseTask = Task.Run(() =>
            {
                if (!CanConnectToDatabase())
                {
                    dispatcherService.InvokeOnUIThread(() =>
                    {
                        Application.Current.Shutdown();
                    });
                }
            });

            var loadComponents = Task.Run(async () =>
            {
                //Load any components you need 


            });

            Task.WhenAll(testDatabaseTask, loadComponents).ContinueWith(t =>
            {
                dispatcherService.InvokeOnUIThread(() =>
                {
                    windowService.ShowWindow<MainViewModel>();
                    splashScreen.Close();
                });
            });


        }

        private bool CanConnectToDatabase()
        {
            var messageBoxService = AppHost!.Services.GetRequiredService<IMessageBoxService>();
            try
            {
                IDbContextFactory<AppDbContext> dbContextFactory = AppHost!.Services.GetRequiredService<IDbContextFactory<AppDbContext>>();
                var dbContext = dbContextFactory.CreateDbContext();
                return true;
            }
            catch (MySqlException ex)
            {
                messageBoxService.ShowError($"Failed to connect to the database: {ex.Message}");
                return false;
            }

        }
        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}

