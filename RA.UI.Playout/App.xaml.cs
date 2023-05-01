﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySqlConnector;
using RA.Database;
using RA.UI.Core.Factories;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.Themes;
using RA.UI.Playout.ViewModels;
using RA.UI.Playout.Views;
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

                    #region Singleton viewModel <=> View
                    viewModelToSingletonWindowMap.Add(typeof(MainViewModel), typeof(MainWindow));
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
            ThemeManager.SetTheme(ThemeType.Light);
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
