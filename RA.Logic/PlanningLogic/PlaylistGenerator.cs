using RA.DAL;
using RA.Database.Models;
using RA.DTO;
using RA.DTO.Abstract;
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
        private readonly IClocksService clocksService;
        private readonly ITemplatesService templatesService;
        private readonly ISchedulesService schedulesService;

        public PlaylistGenerator(IPlaylistsService playlistsService, 
            IClocksService clocksService,
            ITemplatesService templatesService,
            ISchedulesService schedulesService)
        {
            this.playlistsService = playlistsService;
            this.clocksService = clocksService;
            this.templatesService = templatesService;
            this.schedulesService = schedulesService;
        }

        public PlaylistDTO GeneratePlaylistForDate(DateTime date)
        {
            var result = InitialisePlaylist(date);
            DateTime estimatedPlaylistStart = new DateTime(date.Year, date.Month, date.Day);
            IScheduleDTO schedule = schedulesService.GetScheduleByDate(date);
            int scheduleTemplateId = schedule.Template?.Id ?? throw new Exception("Template must have an id");
            var clocksForSchedule = templatesService.GetClocksForTemplate(scheduleTemplateId);

            foreach(var clock in clocksForSchedule)
            {
                TimeSpan clockSpan = new TimeSpan(clock.ClockSpan, 0, 0);
                TimeSpan clockStart = clock.StartTime;
                TimeSpan clockEnd = clockStart.Add(clockSpan);

                Console.WriteLine($"ClockId={clock.ClockId},ClockStart={clockStart},ClockEnd={clockEnd}");
                Console.WriteLine($"Generating playlist for {clock.ClockSpan} consecutive hours");

                var clockItems = clocksService.GetClockItems(clock.ClockId);
                Console.WriteLine($"Current clock has {clockItems.Count()} items");

                int h = 0;
                for (int i = 1; i <= clock.ClockSpan; i++)
                {
                    Console.WriteLine($"Generating for hour {h++}");

                    foreach (ClockItemDTO clockItem in clockItems)
                    {
                        Console.WriteLine($"Id={clockItem.Id},CategoryId={clockItem.CategoryId}");
                    }

                    Console.WriteLine("=======================================");
                }
            }
            return result;
        }

        private PlaylistDTO InitialisePlaylist(DateTime date)
        {
            var playlist = new PlaylistDTO();
            playlist.AirDate = date;
            playlist.DateAdded = DateTime.Now;
            playlist.Items = new List<PlaylistItemBaseDTO>();

            return playlist;
        }
    }
}
