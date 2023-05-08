using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.Database.Models;
using RA.Database;
using RA.UI.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels.Playlists.Models;
using RA.UI.Core.Services;
using RA.DAL;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Playlists
{
    public partial class PlannerGeneratePlaylistsViewModel : DialogViewModelBase
    {
        public delegate void PlaylistGeneratedEventHandler();

        private readonly IDispatcherService dispatcherService;
        private readonly ISchedulesDefaultService schedulesService;

        #region Properties
        public ObservableCollection<ScheduleOverviewModel> ScheduleOverview { get; set; } = new();

        [ObservableProperty]
        private DateTime startDate = DateTime.Now.Date;

        partial void OnStartDateChanged(DateTime value)
        {
            _ = LoadOverview();
        }

        [ObservableProperty]
        private int numberOfDaysToSchedule = 1;

        partial void OnNumberOfDaysToScheduleChanged(int value)
        {
            _ = LoadOverview();
        }

        #endregion

        #region Constructor

        public PlannerGeneratePlaylistsViewModel(IWindowService windowService, IDispatcherService dispatcherService,
            ISchedulesDefaultService schedulesService) : base(windowService)
        {
            this.dispatcherService = dispatcherService;
            this.schedulesService = schedulesService;
            _ = LoadOverview();
        }

        #endregion

        #region Data fetching
        private async Task LoadOverview()
        {
            ScheduleOverview.Clear();
            var scheduleOverview = await Task.Run(() => 
                schedulesService.GetDefaultSchedulesOverviewAsync(StartDate, StartDate.AddDays(NumberOfDaysToSchedule - 1)));

            foreach(var schedule in scheduleOverview)
            {
                var item = ScheduleOverviewModel.FromDto(schedule.Key, schedule.Value);

                if (schedule.Value == null)
                {
                    item.GenerationStatus = ScheduleGenerationStatus.NoScheduleFound;
                }
                ScheduleOverview.Add(item);
            }
        }
        #endregion


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

        protected override void FinishDialog()
        {
            throw new NotImplementedException("Generate playlists and then finish.");
            base.FinishDialog();
        }
        protected override bool CanFinishDialog()
        {
            return false;
        }
    }
}
