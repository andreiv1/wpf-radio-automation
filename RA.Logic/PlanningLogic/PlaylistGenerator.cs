using RA.DAL;
using RA.DAL.Exceptions;
using RA.Database.Models;
using RA.DTO;
using RA.DTO.Abstract;
using RA.Logic.PlanningLogic.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.PlanningLogic
{
    public class PlaylistGenerator : IPlaylistGenerator
    {
        private readonly IPlaylistsService playlistsService;
        private readonly ITracksService tracksService;
        private readonly IClocksService clocksService;
        private readonly ITemplatesService templatesService;
        private readonly ISchedulesService schedulesService;

        public PlaylistGenerator(IPlaylistsService playlistsService,
                                 ITracksService tracksService,
                                 IClocksService clocksService,
                                 ITemplatesService templatesService,
                                 ISchedulesService schedulesService)
        {
            this.playlistsService = playlistsService;
            this.tracksService = tracksService;
            this.clocksService = clocksService;
            this.templatesService = templatesService;
            this.schedulesService = schedulesService;
        }

        public PlaylistDTO GeneratePlaylistForDate(DateTime date)
        {
            PlaylistDTO playlist = InitialisePlaylist(date);
            IScheduleDTO? schedule = schedulesService.GetScheduleByDate(date);

            if (schedule != null)
            {
                int scheduleTemplateId = schedule.Template?.Id ?? throw new Exception("Template must have an id");
                var clocksForSchedule = templatesService.GetClocksForTemplate(scheduleTemplateId);

                foreach (var clock in clocksForSchedule)
                {

                    ProcessClock(clock, playlist);

                }
            }

            return playlist;
        }

        private PlaylistDTO InitialisePlaylist(DateTime date)
        {
            var playlist = new PlaylistDTO();
            playlist.AirDate = date;
            playlist.DateAdded = DateTime.Now;
            playlist.Items = new List<PlaylistItemBaseDTO>();

            return playlist;
        }

        /// <summary>
        /// Handle processing a clock inside a template
        /// </summary>
        /// <param name="clock">The specific clock in the template</param>
        private void ProcessClock(ClockTemplateDTO clock, PlaylistDTO playlistDTO)
        {
            TimeSpan clockSpan = new TimeSpan(clock.ClockSpan, 0, 0);
            TimeSpan clockStart = clock.StartTime;
            TimeSpan clockEnd = clockStart.Add(clockSpan);

            Console.WriteLine($"ClockId={clock.ClockId},ClockStart={clockStart},ClockEnd={clockEnd}");
            Console.WriteLine($"Generating playlist for {clock.ClockSpan} consecutive hours");

            List<ClockItemBaseDTO> clockItems = clocksService.GetClockItems(clock.ClockId).ToList();
            Console.WriteLine($"Current clock has {clockItems.Count()} items");

            int h = 0;
            for (int i = 1; i <= clock.ClockSpan; i++)
            {
                Console.WriteLine($"Generating for hour {h++}");

                foreach (ClockItemBaseDTO clockItem in clockItems)
                {
                    ProcessClockItem(clockItem, playlistDTO);
                }

                Console.WriteLine("=======================================");
            }
        }

        /// <summary>
        /// Handle processing a single item from a clock
        /// </summary>
        /// <param name="clockItem"></param>
        private void ProcessClockItem(ClockItemBaseDTO clockItem, PlaylistDTO playlistDTO)
        {
            //Console.WriteLine($"Id={clockItem.Id},CategoryId={clockItem.CategoryId}");
            //if (clockItem.TrackId.HasValue)
            //{
            //    //TODO: Specific element from library
            //    //no selection to made

            //}

            ////TODO: Event 

            //if (clockItem.CategoryId.HasValue)
            //{
            //    TrackSelectionBaseStrategy selectionStrategy =
            //        new RandomTrackSelectionStrategy(playlistsService, tracksService, clockItem.CategoryId.Value);
            //    selectionStrategy.SelectTrack(playlistDTO);
            //}


        }
    }
}
