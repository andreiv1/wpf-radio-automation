using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RA.UI.Core.Factories;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.Categories;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.ImportItems;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.MainContent;
using RA.UI.StationManagement.Components.MediaLibrary.Views;
using RA.UI.StationManagement.Components.MediaLibrary.Views.Categories;
using RA.UI.StationManagement.Components.MediaLibrary.Views.ImportItems;
using RA.UI.StationManagement.Components.MediaLibrary.Views.MainContent;
using RA.UI.StationManagement.Components.Planner.View.Clocks;
using RA.UI.StationManagement.Components.Planner.View.Playlists;
using RA.UI.StationManagement.Components.Planner.View.Schedule;
using RA.UI.StationManagement.Components.Planner.View.Templates;
using RA.UI.StationManagement.Components.Planner.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels.Clocks;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent;
using RA.UI.StationManagement.Components.Planner.ViewModels.Playlists;
using RA.UI.StationManagement.Components.Planner.ViewModels.Schedule;
using RA.UI.StationManagement.Components.Planner.ViewModels.Templates;
using RA.UI.StationManagement.Components.Planner.Views;
using RA.UI.StationManagement.Components.Planner.Views.Clocks;
using RA.UI.StationManagement.Components.Planner.Views.MainContent;
using RA.UI.StationManagement.Components.Planner.Views.Schedule;
using RA.UI.StationManagement.Components.Planner.Views.Templates;
using RA.UI.StationManagement.Components.Reports.ViewModels;
using RA.UI.StationManagement.Components.Reports.Views;
using RA.UI.StationManagement.Components.Settings.ViewModels;
using RA.UI.StationManagement.Components.Settings.ViewModels.MainContent;
using RA.UI.StationManagement.Components.Settings.ViewModels.Security;
using RA.UI.StationManagement.Components.Settings.Views;
using RA.UI.StationManagement.Components.Settings.Views.MainContent;
using RA.UI.StationManagement.Components.Settings.Views.Security;
using RA.UI.StationManagement.Dialogs.ArtistSelectDialog;
using RA.UI.StationManagement.Dialogs.CategorySelectDialog;
using RA.UI.StationManagement.Dialogs.TemplateSelectDialog;
using RA.UI.StationManagement.Dialogs.TrackFilterDialog;
using RA.UI.StationManagement.Dialogs.TrackSelectDialog;
using System;
using System.Collections.Generic;
using System.Windows;

namespace RA.UI.StationManagement.HostBuilders
{
    public static class AddViewModelsHostBuilderExtensions
    {
        static readonly Dictionary<Type, Type> viewModelToSingletonWindowMap = new()
        {
            { typeof(AuthViewModel), typeof(AuthWindow) },
            { typeof(LauncherViewModel), typeof(LauncherWindow) }
        };
        static readonly Dictionary<Type, Type> viewModelToTransientWindowMap = new()
        {
            { typeof(AllMediaItemsViewModel), typeof(AllMediaItemsView) },
            { typeof(ArtistSelectViewModel), typeof(ArtistSelectDialog) },
            { typeof(ArtistsViewModel), typeof(ArtistsView) },
            { typeof(CategoryItemsViewModel), typeof(CategoryItemsView) },
            { typeof(CategorySelectViewModel), typeof(CategorySelectDialog) },
            { typeof(CategoriesViewModel), typeof(CategoriesView) },
            { typeof(ImportItemsFirstViewModel), typeof(ImportItemsFirstView) },
            { typeof(ImportItemsSecondViewModel), typeof(ImportItemsSecondView) },
            { typeof(ImportItemsThirdViewModel), typeof(ImportItemsThirdView) },
            { typeof(MediaLibraryImportItemsViewModel), typeof(MediaLibraryImportItemsWindow) },
            { typeof(MediaLibraryMainViewModel), typeof(MediaLibraryMainWindow) },
            { typeof(MediaLibraryManageCategoryViewModel), typeof(MediaLibraryManageCategoryWindow) },
            { typeof(MediaLibraryManageTrackViewModel), typeof(MediaLibraryManageTrackWindow) },
            { typeof(PlannerClocksViewModel), typeof(PlannerClocksView) },
            { typeof(PlannerDayTemplatesViewModel), typeof(PlannerDayTemplatesView) },
            { typeof(PlannerDefaultScheduleViewModel), typeof(PlannerDefaultScheduleView) },
            { typeof(PlannerGeneratePlaylistsViewModel), typeof(PlannerGeneratePlaylistsWindow) },
            { typeof(PlannerMainViewModel), typeof(PlannerMainWindow) },
            { typeof(PlannerManageClockCategoryRuleViewModel), typeof(PlannerManageClockCategoryRuleDialog) },
            { typeof(PlannerManageClockEventRuleViewModel), typeof(PlannerManageClockEventRuleDialog) },
            { typeof(PlannerManageClockViewModel), typeof(PlannerManageClockDialog) },
            { typeof(PlannerManageScheduleItemViewModel), typeof(PlannerManageScheduleItemDialog) },
            { typeof(PlannerManageTemplateViewModel), typeof(PlannerManageTemplateDialog) },
            { typeof(PlannerPlaylistsViewModel), typeof(PlannerPlaylistsView) },
            { typeof(PlannerScheduleCalendarViewModel), typeof(PlannerScheduleView) },
            { typeof(PlannerScheduleViewModel), typeof(PlannerScheduleView) },
            { typeof(PlannerTemplateSelectClockViewModel), typeof(PlannerTemplateSelectClockWindow) },
            { typeof(ReportsMainViewModel), typeof(ReportsMainWindow) },
            { typeof(SettingsMainViewModel), typeof(SettingsMainWindow) },
            { typeof(TagsViewModel), typeof(TagsView) },
            { typeof(TemplateSelectViewModel), typeof(TemplateSelectDialog) },
            { typeof(TrackSelectViewModel), typeof(TrackSelectDialog) },
            { typeof(PlannerPreviewClockViewModel), typeof(PlannerPreviewClockWindow) },
            { typeof(TrackFilterViewModel), typeof(TrackFilterDialog) },
            { typeof(SettingsAboutViewModel), typeof(SettingsAboutView) },
            { typeof(SettingsDatabaseViewModel), typeof(SettingsAboutView) },
            { typeof(SettingsGeneralViewModel), typeof(SettingsGeneralView) },
            { typeof(SettingsSecurityViewModel), typeof(SettingsSecurityView) },
            { typeof(SettingsManageGroupViewModel), typeof(SettingsManageGroupDialog) },
            { typeof(SettingsManageUserViewModel), typeof(SettingsManageUserDialog) }
        };
        public static IHostBuilder AddViewModels(this IHostBuilder host)
        {
            return host.ConfigureServices((hostContext, services) =>
            {
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
