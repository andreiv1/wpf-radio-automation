using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.Logic;
using RA.Logic.Tracks;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.ImportItems;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.Models;
using System;
using System.IO;
using System.Linq;
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
                                                IMessageBoxService messageBoxService,
                                                IDispatcherService dispatcherService,
                                                INavigationService<MediaLibraryImportItemsViewModel> navigationService,
                                                ITrackFilesProcessor trackFilesProcessor,
                                                ITrackFilesImporter trackFilesImporter,
                                                ImportItemsFirstViewModel importItemsFirstViewModel,
                                                ImportItemsSecondViewModel importItemsSecondViewModel,
                                                ImportItemsThirdViewModel importItemsThirdViewModel,
                                                int? categoryId = null) : base(navigationService)
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

            if (categoryId.HasValue)
            {
                importItemsFirstViewModel?.SetCategory(categoryId.Value);
            }
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
                if(string.IsNullOrEmpty(Model.FolderPath) || string.IsNullOrEmpty(Model?.SelectedCategory?.PathName))
                {
                    messageBoxService.ShowError($"You forgot to select a folder path and a base category where to import the songs!\n" +
                        $"Let's go back to first page, then you can proceed.");
                    Page = 0;
                    return;
                }

                Model!.ProcessingTracks.Clear();
                ExecuteImportCommand.NotifyCanExecuteChanged();
            }
            else if (viewModelType == typeof(ImportItemsThirdViewModel))
            {
                if(Model.DestinationOption != DestinationOptions.LeaveCurrent)
                {
                    if (string.IsNullOrEmpty(Model.NewDestinationPath))
                    {
                        messageBoxService.ShowError($"If you selected to move/copy the items, you must select a new destination path.\n" +
                            "Let's go back to the second page.");
                        Page = 1;
                        return;
                    }
                }

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

        private TrackFilesProcessorOptions options;
        private async Task HandleProcessTracks()
        {
            
            if (Model.FolderPath != null && Model.SelectedCategory != null)
            {
                Model.IsTrackProcessRunning = true;
 
                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                TrackMetadataReader.ImagePath = Path.Combine(appDataFolder, "RadioAutomationSystem", "images");

                SubfolderScanOption scanOption;
                switch (Model.ScanOptions)
                {
                    case CompleteScanOptions.None:
                        scanOption = SubfolderScanOption.None;
                        break;
                    case CompleteScanOptions.PutItemsInTheSameCategory:
                        scanOption = SubfolderScanOption.PutAllInSameCategory;
                        break;
                    case CompleteScanOptions.CreateNewCategoriesAndAsignItems:
                        scanOption = SubfolderScanOption.CreateNewChildrenCategoryForEachExistingCategory;
                        break;
                    default:
                        throw new NotSupportedException($"The option {Model.ScanOptions} is not supported");
                }

                NewDirectoryOption newDirectoryOption = NewDirectoryOption.LeaveCurrent;
                switch (Model.DestinationOption)
                {
                    case DestinationOptions.LeaveCurrent:
                        newDirectoryOption = NewDirectoryOption.LeaveCurrent;
                        break;
                    case DestinationOptions.CopyToANewLocation:
                        newDirectoryOption = NewDirectoryOption.CopyToNewLocation;
                        break;
                    case DestinationOptions.MoveToANewLocation:
                        newDirectoryOption = NewDirectoryOption.MoveToNewLocation;
                        break;
                  
                }

                options = new TrackFilesProcessorOptionsBuilder(Model.FolderPath, Model.SelectedCategory.Id)
                    .SetReadMetadata(Model.ReadItemsMetadata)
                    .SetTrackStatus(Model.SelectedTrackStatus)
                    .SetTrackType(Model.SelectedTrackType)
                    .SetScanSubfolders(Model.IsCompleteScan)
                    .SetSubfolderScanOption(scanOption)
                    .SetNewDirectoryPath(newDirectoryPath: Model.NewDestinationPath,
                                         newDirectoryOption)
                    .Build();

                dispatcherService.InvokeOnUIThread(() =>
                {
                    Model.Messages.Add($"Analyzing tracks in directory {options.DirectoryPath}...");
                    if(options.NewDirectoryOption != NewDirectoryOption.LeaveCurrent)
                    {
                        Model.Messages.Add($"Items will be {(options.NewDirectoryOption == NewDirectoryOption.CopyToNewLocation ? "copied" : "moved")} " +
                            $"to {options.NewDirectoryPath} after importing.");
                    }
                });


                Model.ValidItems = 0;
                Model.InvalidItems = 0;
                Model.WarningItems = 0;
                Model.ProcessedItems = 0;
                Model.TotalItems = await trackFilesProcessor.CountItemsInDirectoryAsync(options);

                await Task.Run(async () =>
                {
                    await foreach (var processingTrack in trackFilesProcessor.ProcessItemsFromDirectoryAsync(options))
                    {
                        await dispatcherService.InvokeOnUIThreadAsync(() =>
                        {
  
                            switch (processingTrack.Status)
                            {
                                case Logic.Tracks.Enums.ProcessingTrackStatus.OK:
                                    Model.ValidItems++;
                                    break;
                                case Logic.Tracks.Enums.ProcessingTrackStatus.FAILED:
                                    Model.InvalidItems++;
                                    break;
                                case Logic.Tracks.Enums.ProcessingTrackStatus.WARNING:
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
            int result = await trackFilesImporter.ImportAsync(Model.ProcessingTracks, options);


            messageBoxService.ShowYesNoInfo(
                message: $"{result} {(result == 1 ? "track" : "tracks")} has been imported succesfully into database.\n Do you want to make a new import?",
                title: "Pick an option",
                actionYes: () => { Page = 0;}, 
                actionNo: () => { windowService.CloseWindow(this); });
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
