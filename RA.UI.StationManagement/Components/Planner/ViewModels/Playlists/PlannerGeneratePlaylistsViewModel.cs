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
using RA.Logic.Planning;
using RA.DTO;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Playlists
{
    public partial class PlannerGeneratePlaylistsViewModel : DialogViewModelBase
    {
        private readonly IDispatcherService dispatcherService;
        private readonly IMessageBoxService messageBoxService;
        private readonly IPlaylistsService playlistsService;
        private readonly ISchedulesService schedulesService;
        private readonly IPlaylistGenerator playlistGenerator;
        private bool isGeneratingPlaylist = false;

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

 
        public PlannerGeneratePlaylistsViewModel(IWindowService windowService,
                                                 IDispatcherService dispatcherService,
                                                 IMessageBoxService messageBoxService,
                                                 IPlaylistsService playlistsService,
                                                 ISchedulesService schedulesService,
                                                 IPlaylistGenerator playlistGenerator) : base(windowService)
        {
            this.dispatcherService = dispatcherService;
            this.messageBoxService = messageBoxService;
            this.playlistsService = playlistsService;
            this.schedulesService = schedulesService;
            this.playlistGenerator = playlistGenerator;
            _ = LoadOverview();
        }


        //Data fetching
        private async Task LoadOverview()
        {
            var scheduleOverview = await Task.Run(() => schedulesService.GetSchedulesOverview(StartDate, StartDate.AddDays(NumberOfDaysToSchedule - 1)));
            ScheduleOverview.Clear();
            foreach(var schedule in scheduleOverview)
            {
                var item = ScheduleOverviewModel.FromDto(schedule.Key, schedule.Value);
                bool exists = await playlistsService.PlaylistExists(schedule.Key);
                if (exists)
                {
                    item.GenerationStatus = ScheduleGenerationStatus.AlreadyExists;
                } 
                else if (schedule.Value == null)
                {
                    item.GenerationStatus = ScheduleGenerationStatus.NoScheduleFound;
                }
                ScheduleOverview.Add(item);
            }
            FinishDialogCommand.NotifyCanExecuteChanged();
        }

        private void GeneratePlaylists()
        {
            List<PlaylistDTO> generatedPlaylists = new();
            Task.Run(async () =>
            {
                foreach (var item in ScheduleOverview)
                {
                    if (item.GenerationStatus != ScheduleGenerationStatus.NoScheduleFound && item.GenerationStatus != ScheduleGenerationStatus.AlreadyExists)
                    {
                        item.GenerationStatus = ScheduleGenerationStatus.Generating;
                        try
                        {
                            var p = await playlistGenerator.GeneratePlaylistForDate(item.Date);
                            item.GenerationStatus = ScheduleGenerationStatus.Generated;
                            generatedPlaylists.Add(p);
                            await playlistsService.AddPlaylistAsync(p);
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
        protected override void FinishDialog()
        {
            isGeneratingPlaylist = true;
            FinishDialogCommand.NotifyCanExecuteChanged();
            GeneratePlaylists();
        }

        private bool CanGenerateAny()
        {
            foreach(var item in ScheduleOverview)
            {
                if (item.GenerationStatus == ScheduleGenerationStatus.NotGenerated)
                {
                    return true;
                }
            }
            return false;
        }
        protected override bool CanFinishDialog()
        {
            return !isGeneratingPlaylist && CanGenerateAny();
        }
    }
}
