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
        private readonly IClocksService clocksService;
        private readonly ITemplatesService templatesService;
        private readonly ISchedulesService schedulesService;

        public PlaylistGenerator(IClocksService clocksService,
                                 ITemplatesService templatesService,
                                 ISchedulesService schedulesService)
        {
            this.clocksService = clocksService;
            this.templatesService = templatesService;
            this.schedulesService = schedulesService;
        }

        public PlaylistDTO GeneratePlaylistForDate(DateTime date)
        {
            PlaylistDTO playlist = PlaylistDTO.Initialise(date);
            IScheduleDTO? schedule = schedulesService.GetScheduleByDate(date);

            if(schedule != null)
            {
                int scheduleTemplateId = schedule.Template?.Id ?? throw new Exception("Template must have an id");
                var clocksForSchedule = templatesService.GetClocksForTemplate(scheduleTemplateId);

                Console.WriteLine($"Trying to generate playlist for {date.ToString("dd/mm/yyyy")} with template: {schedule.Template.Name}");

                foreach (var clock in clocksForSchedule)
                {
                    _ = ProcessClock(clock, playlist);
                }
               
            }

            return playlist;
        }

        private async Task ProcessClock(ClockTemplateDTO clock, PlaylistDTO playlist)
        {
            TimeSpan clockSpan = new TimeSpan(clock.ClockSpan, 0, 0);
            TimeSpan clockStart = clock.StartTime;
            TimeSpan clockEnd = clockStart.Add(clockSpan);

            Console.WriteLine($"ClockId={clock.ClockId},ClockStart={clockStart},ClockEnd={clockEnd},ConsecutiveHours={clock.ClockSpan}");
            List<ClockItemBaseDTO> clockItems = clocksService.GetClockItems(clock.ClockId).ToList();
            ShowClockItems(clockItems);

            int h = 0;
            for (int i = 1; i <= clock.ClockSpan; i++)
            {
                Console.WriteLine($"Generating for hour {h++}");

            }
        }


        /// <summary>
        /// For debug in console
        /// </summary>
        /// <param name="clockItems"></param>
        private void ShowClockItems(IEnumerable<ClockItemBaseDTO> clockItems)
        {
            Console.WriteLine($"Current clock has {clockItems.Count()} items");

            var regularClockItems = clockItems.Where(ci => ci.OrderIndex >= 0).ToList();

            //Contain events and sub-items for events that should be played at a specific time
            var specialClockItems = clockItems.Where(ci => ci.OrderIndex < 0).ToList();

            Console.WriteLine("Special items (events + event's items): ");
            foreach (ClockItemBaseDTO clockItem in specialClockItems.Where(ci => !ci.ClockItemEventId.HasValue).ToList())
            {
                Console.WriteLine($"Id={clockItem.Id},OrderIndex={clockItem.OrderIndex}");
                foreach (ClockItemBaseDTO subItem in specialClockItems.Where(ci => ci.ClockItemEventId == clockItem.Id).ToList())
                {
                    Console.WriteLine($">>> Id={subItem.Id},OrderIndex={subItem.OrderIndex},EventOrderIndex={subItem.EventOrderIndex}");
                }
            }
            Console.WriteLine("Regular items: ");
            foreach (ClockItemBaseDTO clockItem in regularClockItems)
            {
                Console.WriteLine($"Id={clockItem.Id},OrderIndex={clockItem.OrderIndex}");
            }
        }

    }
}
