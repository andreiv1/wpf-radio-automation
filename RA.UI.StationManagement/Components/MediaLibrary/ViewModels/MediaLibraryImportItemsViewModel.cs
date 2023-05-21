using Castle.DynamicProxy.Contributors;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using RA.Logic;
using RA.Logic.TrackFileLogic;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.ImportItems;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.Models;
using RA.UI.StationManagement.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RA.UI.StationManagement.Components.MediaLibrary.ViewModels
{
    public partial class MediaLibraryImportItemsViewModel : PagedViewModel<MediaLibraryImportItemsViewModel>
    {
        private readonly IWindowService windowService;
        private readonly IDispatcherService dispatcherService;
        private readonly INavigationService<MediaLibraryImportItemsViewModel> navigationService;
        private readonly ITrackFilesProcessor trackFilesProcessor;
        private readonly ITrackFilesImporter trackFilesImporter;

        [ObservableProperty]
        private ImportItemsModel model = new();
        public MediaLibraryImportItemsViewModel(IWindowService windowService, IDispatcherService dispatcherService,
            INavigationService<MediaLibraryImportItemsViewModel> navigationService,
            ITrackFilesProcessor trackFilesProcessor, ITrackFilesImporter trackFilesImporter) : base(navigationService)
        {
            PageChanged += MediaLibraryImportItemsViewModel_PageChanged;
            this.windowService = windowService;
            this.dispatcherService = dispatcherService;
            this.navigationService = navigationService;
            this.trackFilesProcessor = trackFilesProcessor;
            this.trackFilesImporter = trackFilesImporter;
        }
        protected override void InitialisePages()
        {
            var firstVm = App.AppHost!.Services.GetRequiredService<ImportItemsFirstViewModel>();
            var secondVm = App.AppHost!.Services.GetRequiredService<ImportItemsSecondViewModel>();
            var thirdVm = App.AppHost!.Services.GetRequiredService<ImportItemsThirdViewModel>();
            firstVm.Model = Model;
            secondVm.Model = Model;
            thirdVm.Model = Model;

            viewModels.Add(firstVm);
            viewModels.Add(secondVm);
            viewModels.Add(thirdVm);
        }
        private void MediaLibraryImportItemsViewModel_PageChanged(object sender, int newPageIndex)
        {
            DebugHelper.WriteLine(this,$"Page changed to {newPageIndex}");
            Type viewModelType = viewModels.ElementAt(newPageIndex).GetType();
            if (viewModelType == typeof(ImportItemsFirstViewModel))
            {

            }
            else if (viewModelType == typeof(ImportItemsSecondViewModel))
            {
                Model.ProcessingTracks.Clear();
                ExecuteImportCommand.NotifyCanExecuteChanged();
            }
            else if (viewModelType == typeof(ImportItemsThirdViewModel))
            {
                Model.Messages.Clear();
                Task.Run(() =>
                {
                    Task processTask = HandleProcessTracks();

                    processTask.ContinueWith(t =>
                    {
                        //Use dispatcher service
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            GoToPreviousPageCommand.NotifyCanExecuteChanged();
                            ExecuteImportCommand.NotifyCanExecuteChanged();
                        }));
                    });
                });

            }
        }

        private async Task HandleProcessTracks()
        {
            
            if (Model.FolderPath != null && Model.SelectedCategory != null)
            {
                Model.IsTrackProcessRunning = true;
                dispatcherService.InvokeOnUIThread(() =>
                {
                    Model.Messages.Add("Started the process of importing...");
                });

                //TODO: put it in settings
                TrackMetadataReader.ImagePath = @"C:\Users\Andrei\Desktop\images";
                TrackFilesProcessorOptions options = new TrackFilesProcessorOptionsBuilder(Model.FolderPath, Model.SelectedCategory.Id)
                    .SetReadMetadata(Model.ReadItemsMetadata)
                    .SetTrackStatus(Model.SelectedTrackStatus)
                    .SetTrackType(Model.SelectedTrackType)
                    .Build();

                await Task.Run(async () =>
                {
                    await foreach (var processingTrack in trackFilesProcessor.ProcessItemsFromDirectoryAsync(options))
                    {
                        dispatcherService.InvokeOnUIThread(() =>
                        {
                            Model.ProcessingTracks.Add(processingTrack);
                        });
                        
                    }
                });    
            }
            Model.IsTrackProcessRunning = false;
        }

        [RelayCommand(CanExecute = nameof(CanExecuteImport))]
        private async Task ExecuteImport()
        {
            await trackFilesImporter.ImportAsync(Model.ProcessingTracks);
            dispatcherService.InvokeOnUIThread(() =>
            {
                Model.Messages.Add($"Tracks has been imported succesfully into database.");
            });

        }

        private bool CanExecuteImport()
        {
            return viewModels.ElementAt(Page).GetType() == typeof(ImportItemsThirdViewModel) 
                && !Model.IsTrackProcessRunning;
        }

        public override void Dispose()
        {
            
            PageChanged -= MediaLibraryImportItemsViewModel_PageChanged;
            foreach(var vm in viewModels)
            {
                vm.Dispose();
            }
            base.Dispose();
        }
    }
}
