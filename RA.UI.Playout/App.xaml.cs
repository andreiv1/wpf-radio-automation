using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            ThemeManager.SetTheme(ThemeType.Dark);
            SplashScreenWindow splashScreen = new SplashScreenWindow();
            splashScreen.Show();

            //var canConnect = false;
            //var testDatabaseTask = Task.Run(() =>
            //{
            //    canConnect = CanConnectToDatabase();
            //});



            //Task.WhenAll(testDatabaseTask, loadComponents).ContinueWith(t =>
            //{
            //    dispatcherService.InvokeOnUIThread(() =>
            //    {
            //        if (canConnect)
            //        {
            //            windowService.ShowWindow<MainViewModel>();
            //        }
            //        else
            //        {
            //            windowService.ShowWindow<DatabaseSetupViewModel>();
            //        }

            //        splashScreen.Hide();
            //    });
            //});
            var loadingTask = Task.Run(async () =>
            {
                await Task.Delay(1000);
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

        //private bool CanConnectToDatabase()
        //{
        //    var messageBoxService = AppHost!.Services.GetRequiredService<IMessageBoxService>();
        //    try
        //    {
        //        IDbContextFactory<AppDbContext> dbContextFactory = AppHost!.Services.GetRequiredService<IDbContextFactory<AppDbContext>>();
        //        using var dbContext = dbContextFactory.CreateDbContext();
        //        var result = dbContext.Database.ExecuteSqlRaw("SELECT 1 FROM Tracks;");
        //        return true;
        //    }
        //    catch (MySqlException ex)
        //    {
        //        messageBoxService.ShowError($"Failed to connect to the database: {ex.Message}");
        //        return false;
        //    }
        //    catch (Exception )
        //    {
        //        return false;
        //    }

        //}
        protected override async void OnExit(ExitEventArgs e)
        {
            await host.StopAsync();
            base.OnExit(e);
        }
    }
}

