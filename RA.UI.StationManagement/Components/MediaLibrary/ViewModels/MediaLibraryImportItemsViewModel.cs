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
using System.IO;
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
        private readonly IMessageBoxService messageBoxService;
        private readonly ITrackFilesProcessor trackFilesProcessor;
        private readonly ITrackFilesImporter trackFilesImporter;

        [ObservableProperty]
        private ImportItemsModel model = new();
        public MediaLibraryImportItemsViewModel(IWindowService windowService,
                                                IDispatcherService dispatcherService,
                                                INavigationService<MediaLibraryImportItemsViewModel> navigationService,
                                                IMessageBoxService messageBoxService,
                                                ITrackFilesProcessor trackFilesProcessor,
                                                ITrackFilesImporter trackFilesImporter,
                                                ImportItemsFirstViewModel importItemsFirstViewModel,
                                                ImportItemsSecondViewModel importItemsSecondViewModel,
                                                ImportItemsThirdViewModel importItemsThirdViewModel) : base(navigationService)
        {
            PageChanged += MediaLibraryImportItemsViewModel_PageChanged;
            this.windowService = windowService;
            this.dispatcherService = dispatcherService;
            this.navigationService = navigationService;
            this.messageBoxService = messageBoxService;
            this.trackFilesProcessor = trackFilesProcessor;
            this.trackFilesImporter = trackFilesImporter;

            viewModels.Add(importItemsFirstViewModel);
            viewModels.Add(importItemsSecondViewModel);
            viewModels.Add(importItemsThirdViewModel);

            importItemsFirstViewModel.Model = model;
            importItemsSecondViewModel.Model = model;
            importItemsThirdViewModel.Model = model;

            Page = 0;
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

        
                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                TrackMetadataReader.ImagePath = Path.Combine(appDataFolder, "RadioAutomationSystem", "images");

                SubfolderScanOption option;
                switch (Model.ScanOptions)
                {
                    case CompleteScanOptions.None:
                        option = SubfolderScanOption.None;
                        break;
                    case CompleteScanOptions.PutItemsInTheSameCategory:
                        option = SubfolderScanOption.PutAllInSameCategory;
                        break;
                    case CompleteScanOptions.CreateNewCategoriesAndAsignItems:
                        option = SubfolderScanOption.CreateNewChildrenCategoryForEachExistingCategory;
                        break;
                    default:
                        throw new NotSupportedException($"The option {Model.ScanOptions} is not supported");
                }

                TrackFilesProcessorOptions options = new TrackFilesProcessorOptionsBuilder(Model.FolderPath, Model.SelectedCategory.Id)
                    .SetReadMetadata(Model.ReadItemsMetadata)
                    .SetTrackStatus(Model.SelectedTrackStatus)
                    .SetTrackType(Model.SelectedTrackType)
                    .SetScanSubfolders(Model.IsCompleteScan)
                    .SetSubfolderScanOption(option)
                    .Build();


                Model.ValidItems = 0;
                Model.InvalidItems = 0;
                Model.WarningItems = 0;
                Model.ProcessedItems = 0;
                Model.TotalItems = await trackFilesProcessor.CountItemsInDirectoryAsync(options);
                await Task.Run(async () =>
                {
                    await foreach (var processingTrack in trackFilesProcessor.ProcessItemsFromDirectoryAsync(options))
                    {
                        dispatcherService.InvokeOnUIThread(() =>
                        {
  
                            switch (processingTrack.Status)
                            {
                                case Logic.TrackFileLogic.Enums.ProcessingTrackStatus.OK:
                                    Model.ValidItems++;
                                    break;
                                case Logic.TrackFileLogic.Enums.ProcessingTrackStatus.FAILED:
                                    Model.InvalidItems++;
                                    break;
                                case Logic.TrackFileLogic.Enums.ProcessingTrackStatus.WARNING:
                                    Model.WarningItems++;
                                    break;
                            }
                            Model.ProcessedItems++;
                            Model.ProcessingTracks.Add(processingTrack);
                            if(!string.IsNullOrEmpty(processingTrack.Message))
                            {
                                Model.Messages.Add($"{processingTrack.Message}");
                            }
                        });
                        
                    }
                });    
            }
            Model.IsTrackProcessRunning = false;
        }

        [RelayCommand(CanExecute = nameof(CanExecuteImport))]
        private async Task ExecuteImport()
        {
            int result = await trackFilesImporter.ImportAsync(Model.ProcessingTracks);
            messageBoxService.ShowYesNoInfo($"{result} {(result == 1 ? "track" : "tracks")} has been imported succesfully into database.\n Do you want to make a new import?",
                "Pick an option",
                () =>
                {
                    //Yes
                    Page = 0;
                }, () =>
                {
                    //No
                    windowService.CloseWindow(this);
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
