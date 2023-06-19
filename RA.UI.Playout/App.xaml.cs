using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RA.Database;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.Shared;
using RA.UI.Core.Themes;
using RA.UI.Playout.HostBuilders;
using RA.UI.Playout.ViewModels;
using System.Threading.Tasks;
using System.Windows;

namespace RA.UI.Playout
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
            var windowService = host.Services.GetRequiredService<IWindowService>();
            var dispatcherService = host.Services.GetRequiredService<IDispatcherService>();
            var messageBoxService = host.Services.GetRequiredService<IMessageBoxService>();

            ThemeManager.SetTheme(ThemeType.Dark);
            SplashScreenWindow splashScreen = new SplashScreenWindow();
            splashScreen.Show();

            var canConnect = false;
            var loadingTask = Task.Run(async () =>
            {
                var dbContextFactory = host.Services.GetRequiredService<IDbContextFactory<AppDbContext>>();
                var dbContext = dbContextFactory.CreateDbContext();

                try
                {
                    canConnect = await DatabaseConnectionTester.CanConnectToDatabase(dbContext);
                }
                catch (DatabaseConnectionException ex)
                {
                    messageBoxService.ShowError(ex.Message);
                    await dispatcherService.InvokeOnUIThreadAsync(() => Application.Current.Shutdown());
                }

            });

            Task.WhenAll(loadingTask).ContinueWith(async t =>
            {
                await dispatcherService.InvokeOnUIThreadAsync(() =>
                {
                    windowService.ShowWindow<MainViewModel>();
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

