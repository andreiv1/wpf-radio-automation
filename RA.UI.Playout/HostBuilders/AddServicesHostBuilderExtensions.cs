using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RA.DAL;
using RA.Logic.AudioPlayer;
using RA.Logic.AudioPlayer.Interfaces;
using RA.Logic.PlanningLogic;
using RA.Logic.TrackFileLogic;
using RA.UI.Core.Factories;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Playout.HostBuilders
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddSingleton<IWindowService, WindowService>();
                services.AddSingleton<IViewModelFactory, ViewModelFactory>();

                services.AddSingleton<IFolderBrowserDialogService, FolderBrowserDialogService>();
                services.AddSingleton<IFileBrowserDialogService, FileBrowserDialogService>();
                services.AddSingleton<IMessageBoxService, MessageBoxService>();
                services.AddSingleton<IDispatcherService, WpfDispatcherService>();

                services.AddSingleton<IAudioPlayer, Player>();
                services.AddSingleton<IPlaybackQueue, PlaybackQueue>();

                services.AddTransient<IPlaylistsService, PlaylistsService>();
                services.AddTransient<ITracksService, TracksService>();
            });

            return host;
        }
    }
}
