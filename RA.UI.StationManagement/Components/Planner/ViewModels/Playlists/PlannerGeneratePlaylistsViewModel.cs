using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.Database.Models;
using RA.Database;
using RA.Logic.PlanningLogic.Interfaces;
using RA.UI.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels.Playlists.Models;
using RA.UI.Core.Services;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Playlists
{
    public partial class PlannerGeneratePlaylistsViewModel : DialogViewModelBase
    {
        public delegate void PlaylistGeneratedEventHandler();

        private readonly IDispatcherService dispatcherService;

        public event PlaylistGeneratedEventHandler? PlaylistGenerated;
        public ObservableCollection<ScheduleOverviewModel> ScheduleOverview { get; set; } = new();

        [ObservableProperty]
        private int numberOfDaysToSchedule = 1;

        public PlannerGeneratePlaylistsViewModel(IWindowService windowService, IDispatcherService dispatcherService) : base(windowService)
        {
            this.dispatcherService = dispatcherService;
            //this.scheduleManager = scheduleManager;
            //this.playlistGenerator = playlistGenerator;
            //_ = LoadOverview();
        }

        protected override bool CanFinishDialog()
        {
            return false;
        }

        //partial void OnNumberOfDaysToScheduleChanged(int value)
        //{
        //    _ = LoadOverview();
        //}

        //[ObservableProperty]
        //private DateTime startDate = DateTime.Now.Date;

        //partial void OnStartDateChanged(DateTime value)
        //{
        //    _ = LoadOverview();
        //}

        //private async Task LoadOverview()
        //{
        //    ScheduleOverview.Clear();
        //    var scheduleOverview = await scheduleManager.GetDefaultScheduleOverviewAsync(StartDate, StartDate.AddDays(NumberOfDaysToSchedule - 1));
        //    foreach (var schedule in scheduleOverview)
        //    {
        //        ScheduleOverview.Add(ScheduleOverviewModel.FromDto(schedule.Key, schedule.Value));
        //    }
        //}

        //[ObservableProperty]
        //private bool isGeneratingStarted = false;

        //[RelayCommand(CanExecute = nameof(CanGeneratePlaylists))]
        //private void GeneratePlaylists()
        //{
        //    IsGeneratingStarted = true;
        //    GeneratePlaylistsCommand.NotifyCanExecuteChanged();
        //    Task.Run(() =>
        //    {
        //        using (var db = new AppDbContext())
        //        {
        //            List<Playlist> generatedPlaylists = new();

        //            foreach (var item in ScheduleOverview)
        //            {

        //                item.GenerationStatus = ScheduleGenerationStatus.Generating;
        //                try
        //                {
        //                    var p = playlistGenerator.GeneratePlaylist(db, item.Date);
        //                    item.GenerationStatus = ScheduleGenerationStatus.Generated;
        //                    generatedPlaylists.Add(p);
        //                    PlaylistGenerated?.Invoke();
        //                }
        //                catch (Exception ex)
        //                {
        //                    item.GenerationStatus = ScheduleGenerationStatus.Error;
        //                    item.ErrorMessage = ex.Message;
        //                }

        //            }

        //            db.Playlists.AddRange(generatedPlaylists);
        //            db.SaveChanges();
        //            IsGeneratingStarted = false;

        //            dispatcherService.InvokeOnUIThread(() =>
        //            {
        //                GeneratePlaylistsCommand.NotifyCanExecuteChanged();
        //            });
        //        }
        //    });
        //}

        //private bool CanGeneratePlaylists()
        //{
        //    return !IsGeneratingStarted;
        //}

        //[RelayCommand(CanExecute = nameof(CanGeneratePlaylists))]
        //private void Cancel()
        //{
        //    windowService.CloseDialog();
        //}
    }
}
