using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.Themes;
using RA.UI.StationManagement.HostBuilders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using RA.UI.Core.Shared;

namespace RA.UI.StationManagement
{
    public partial class App : Application
    {
        private readonly IHost host;

        public App()
        {
            SyncfusionLicense.Register();
            this.host = CreateHostBuilder()
                .Build();    
        }

        public static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .AddConfiguration()
                .AddDbContext()
                .AddServices()
                .AddViewModels();
        }

        protected override async void OnStartup(StartupEventArgs e)
        { 
            await host.StartAsync();
            StartApp();
            base.OnStartup(e);
        }

        private void StartApp()
        {
            ThemeManager.SetTheme(ThemeType.Light);

            var windowService = host.Services.GetRequiredService<IWindowService>();
            var dispatcherService = host.Services.GetRequiredService<IDispatcherService>();

            var splashScreen = new SplashScreenWindow();
            splashScreen.Show();

            var loadingTask = Task.Run(async () =>
            {
                await Task.Delay(1000);
            });

            Task.WhenAll(loadingTask).ContinueWith(async t =>
            {
                await dispatcherService.InvokeOnUIThreadAsync(() =>
                {
                    windowService.ShowWindow<AuthViewModel>();
                    splashScreen.Hide();
                });
            });
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await host.StopAsync();
            base.OnExit(e);
        }
        
    }
}
