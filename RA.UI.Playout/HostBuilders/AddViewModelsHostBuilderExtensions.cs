using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RA.UI.Core.Factories;
using RA.UI.Playout.ViewModels.Components;
using RA.UI.Playout.ViewModels;
using RA.UI.Playout.Views.Components;
using RA.UI.Playout.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RA.UI.Playout.Dialogs.TrackFilterDialog;
using RA.UI.Playout.Dialogs.CategorySelectDialog;

namespace RA.UI.Playout.HostBuilders
{
    public static class AddViewModelsHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder host)
        {
            return host.ConfigureServices((hostContext, services) =>
            {
                Dictionary<Type, Type> viewModelToSingletonWindowMap = new()
                {
                    { typeof(MainViewModel), typeof(MainWindow) },
                    { typeof(PlaylistViewModel), typeof(PlaylistView) },
                    { typeof(NowPlayingViewModel), typeof(NowPlayingView) },
                    { typeof(MediaItemsViewModel), typeof(MediaItemsView) },
                    { typeof(HistoryViewModel), typeof(HistoryView) },
                };
                Dictionary<Type, Type> viewModelToTransientWindowMap = new()
                {
                    {typeof(TrackFilterViewModel), typeof(TrackFilterDialog) },
                    {typeof(CategorySelectViewModel), typeof(CategorySelectDialog) },
                };
            

                //Register window factory
                services.AddSingleton<IWindowFactory>(provider =>
                    new WindowFactory(
                        viewModelToSingletonWindowMap,
                        viewModelToTransientWindowMap,
                        provider.GetRequiredService<IServiceProvider>()
                    )
                );

                //Key: ViewModel, //Value: View
                foreach (KeyValuePair<Type, Type> vmAndView in viewModelToSingletonWindowMap)
                {
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



                foreach (KeyValuePair<Type, Type> vmAndView in viewModelToTransientWindowMap)
                {
                    services.AddTransient(vmAndView.Key);
                    services.AddTransient(vmAndView.Value);
                }

            });
        }
    }
}
