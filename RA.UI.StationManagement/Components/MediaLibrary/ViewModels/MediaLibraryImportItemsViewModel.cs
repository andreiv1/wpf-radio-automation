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
        [ObservableProperty]
        private ImportItemsModel model = new();
        public MediaLibraryImportItemsViewModel(IWindowService windowService, 
            INavigationService<MediaLibraryImportItemsViewModel> navigationService) : base(navigationService)
        {
            PageChanged += MediaLibraryImportItemsViewModel_PageChanged;
            this.windowService = windowService;
            this.navigationService = navigationService;
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
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            GoToPreviousPageCommand.NotifyCanExecuteChanged();
                            ExecuteImportCommand.NotifyCanExecuteChanged();
                        }));
                    });
                });

            }
        }

        private List<ProcessingTrack> processingTracks;

        private TrackFilesProcessor trackFilesProcessor;
        private readonly IWindowService windowService;
        private readonly INavigationService<MediaLibraryImportItemsViewModel> navigationService;

        private async Task HandleProcessTracks()
        {
            Model.IsTrackProcessRunning = true;
            DebugHelper.WriteLine(this, "Starting processing tracks...");
            trackFilesProcessor = new();
            trackFilesProcessor.TrackProcessed += TrackFilesProcessor_TrackProcessed;
            if (Model.FolderPath is not null && Model.SelectedCategory is not null)
            {
                //processingTracks = trackFilesProcessor.ProcessItemsFromDirectory(TrackModel.FolderPath, TrackModel.SelectedCategory.Id,
                //    TrackModel.SelectedTrackType, TrackModel.SelectedTrackStatus, TrackModel.ReadItemsMetadata);
            }

            //Processed finished
            Model.IsTrackProcessRunning = false;
        }

        private void TrackFilesProcessor_TrackProcessed(object sender, Logic.TrackFileLogic.Interfaces.ProcessingTrackEventArgs e)
        {
            Model.TotalItems = trackFilesProcessor.TotalItems;
            Model.ValidItems = trackFilesProcessor.ValidItems;
            Model.InvalidItems = trackFilesProcessor.InvalidItems;
            Model.WarningItems = trackFilesProcessor.WarningItems;

            Application.Current.Dispatcher.Invoke(() =>
            {
                Model.ProcessingTracks.Add(e.Track);
            });
        }

        [RelayCommand(CanExecute = nameof(CanExecuteImport))]
        private void ExecuteImport()
        {
            Task.Run(() =>
            {
                Application.Current?.Dispatcher.Invoke(() =>
                {
                    Model.Messages.Add("Started the process of importing...");
                });
                
                TrackFilesImporter importer = new TrackFilesImporter();
                int result = importer.Import(processingTracks);
                Application.Current?.Dispatcher.Invoke(() =>
                {
                    Model.Messages.Add($"{result} tracks has been imported succesfully into database.");
                });
            });
        }

        private bool CanExecuteImport()
        {
            return viewModels.ElementAt(Page).GetType() == typeof(ImportItemsThirdViewModel) 
                && !Model.IsTrackProcessRunning;
        }

        protected override void Dispose(bool disposing)
        {
            //TODO dispose
            base.Dispose(disposing);
            PageChanged -= MediaLibraryImportItemsViewModel_PageChanged;
        }
    }
}
