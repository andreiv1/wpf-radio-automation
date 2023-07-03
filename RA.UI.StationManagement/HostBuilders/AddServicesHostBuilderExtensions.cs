using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RA.DAL;
using RA.DAL.Interfaces;
using RA.Logic.Planning;
using RA.Logic.Tracks;
using RA.UI.Core.Factories;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels;
using RA.UI.StationManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.HostBuilders
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

                //Navigation services
                services.AddScoped<INavigationService<MediaLibraryMainViewModel>,
                    NavigationService<MediaLibraryMainViewModel>>();

                services.AddTransient<INavigationService<MediaLibraryImportItemsViewModel>,
                    NavigationService<MediaLibraryImportItemsViewModel>>();

                services.AddScoped<MediaLibraryTreeMenuService>();

                // DAL services
                services.AddTransient<IClocksService, ClocksService>();
                services.AddTransient<ICategoriesService, CategoriesService>();
                services.AddTransient<ITracksService, TracksService>();
                services.AddTransient<ITemplatesService, TemplatesService>();
                services.AddTransient<ICategoriesService, CategoriesService>();
                services.AddTransient<ITagsService, TagsService>();
                services.AddTransient<ISchedulesDefaultService, SchedulesDefaultService>();
                services.AddTransient<IArtistsService, ArtistsService>();
                services.AddTransient<ISchedulesService, SchedulesService>();
                services.AddTransient<ISchedulesPlannedService, SchedulesPlannedService>();
                services.AddTransient<IPlaylistsService, PlaylistsService>();
                services.AddTransient<IUsersService, UsersService>();
                services.AddTransient<ITrackHistoryService, TrackHistoryService>(); 

                // Logic services
                services.AddTransient<ITrackFilesImporter, TrackFileImporter>();
                services.AddTransient<ITrackFilesProcessor, TrackFilesProcessor>();
                services.AddTransient<IPlaylistGenerator, PlaylistGeneratorOld>();
            });

            return host;
        }
    }
}
