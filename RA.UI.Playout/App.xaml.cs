using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySqlConnector;
using RA.Database;
using RA.UI.Core.Factories;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.Themes;
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
        public static IHost? AppHost { get; private set; }
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTc3ODYwNUAzMjMxMmUzMTJlMzMzNWc2NlM4V1ZTSm9xU0htNmhzSE1GZDBicktpMWZKY1VoaHdMb3pSaWhmclU9");


            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContextFactory<AppDbContext>(options =>
                    {
                        String connString = "server=192.168.200.113;Port=3306;database=rasoftware;user=root;password=andrewyw1412";
                        options.UseMySql(connString, ServerVersion.AutoDetect(connString))
                            .EnableSensitiveDataLogging(false);
                    });

                    services.AddSingleton<IWindowService, WindowService>();
                    services.AddSingleton<IViewModelFactory, ViewModelFactory>();
                    services.AddSingleton<IMessageBoxService, MessageBoxService>();
                    services.AddSingleton<IDispatcherService, WpfDispatcherService>();

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
                await Task.Delay(50000);
                
                //var window = AppHost!.Services.GetRequiredService<MediaLibraryMainWindow>();
                //window.Show();


            });

            Task.WhenAll(testDatabaseTask, loadComponents).ContinueWith(t =>
            {
                dispatcherService.InvokeOnUIThread(() =>
                {
                    //windowService.ShowWindow<LauncherViewModel>();
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

