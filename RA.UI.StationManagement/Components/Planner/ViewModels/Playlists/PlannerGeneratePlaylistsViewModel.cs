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
using RA.Logic.PlanningLogic;
using RA.DTO;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Playlists
{
    public partial class PlannerGeneratePlaylistsViewModel : DialogViewModelBase
    {
        private readonly IDispatcherService dispatcherService;
        private readonly IMessageBoxService messageBoxService;
        private readonly IPlaylistsService playlistsService;
        private readonly ISchedulesDefaultService schedulesService;
        private readonly IPlaylistGenerator playlistGenerator;
        private bool isGeneratingPlaylist = false;

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
            IMessageBoxService messageBoxService, IPlaylistsService playlistsService,
            ISchedulesDefaultService schedulesService, IPlaylistGenerator playlistGenerator) : base(windowService)
        {
            this.dispatcherService = dispatcherService;
            this.messageBoxService = messageBoxService;
            this.playlistsService = playlistsService;
            this.schedulesService = schedulesService;
            this.playlistGenerator = playlistGenerator;
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

        private void GeneratePlaylists()
        {
            List<PlaylistDTO> generatedPlaylists = new();
            Task.Run(() =>
            {
                foreach (var item in ScheduleOverview)
                {
                    if (item.GenerationStatus != ScheduleGenerationStatus.NoScheduleFound)
                    {
                        item.GenerationStatus = ScheduleGenerationStatus.Generating;
                        try
                        {
                            var p = playlistGenerator.GeneratePlaylistForDate(item.Date);
                            item.GenerationStatus = ScheduleGenerationStatus.Generated;
                            generatedPlaylists.Add(p);
                            _ = playlistsService.AddPlaylistAsync(p);
                        }
                        catch (Exception ex)
                        {
                            item.GenerationStatus = ScheduleGenerationStatus.Error;
                            item.ErrorMessage = ex.Message;
                        }
                    }
                }


            });
        }
        protected override async void FinishDialog()
        {
            isGeneratingPlaylist = true;
            FinishDialogCommand.NotifyCanExecuteChanged();
            GeneratePlaylists();

            //messageBoxService.ShowInfo($"Playlists generated!");

            //base.FinishDialog();
        }
        protected override bool CanFinishDialog()
        {
            return !isGeneratingPlaylist;
        }
    }
}
