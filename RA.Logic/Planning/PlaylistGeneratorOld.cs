using RA.DAL;
using RA.DTO;
using RA.DTO.Abstract;
using RA.Logic.Planning.Abstract;

namespace RA.Logic.Planning
{
    public class PlaylistGeneratorOld : IPlaylistGenerator
    {
        private readonly IPlaylistsService playlistsService;
        private readonly ITracksService tracksService;
        private readonly IClocksService clocksService;
        private readonly ITemplatesService templatesService;
        private readonly ISchedulesService schedulesService;

        public PlaylistGeneratorOld(
            IPlaylistsService playlistsService,
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
            PlaylistDTO playlist = PlaylistDTO.Initialise(date);
            IScheduleDTO? schedule = schedulesService.GetScheduleByDate(date);

            if(schedule != null)
            {
                int scheduleTemplateId = schedule.Template?.Id ?? throw new Exception("Template must have an id");
                var clocksForSchedule = templatesService.GetClocksForTemplate(scheduleTemplateId);

                Console.WriteLine($"Trying to generate playlist for {date.ToString("dd/mm/yyyy")} with template: {schedule.Template.Name}");

                foreach (var clock in clocksForSchedule)
                {
                    ProcessClock(clock, playlist);
                }
               
            }

            return playlist;
        }

        private void ProcessClock(ClockTemplateDTO clock, PlaylistDTO playlist)
        {
            TimeSpan clockSpan = new TimeSpan(clock.ClockSpan, 0, 0);
            TimeSpan clockStart = clock.StartTime;
            TimeSpan clockEnd = clockStart.Add(clockSpan);

            Console.WriteLine($"ClockId={clock.ClockId},ClockStart={clockStart},ClockEnd={clockEnd},ConsecutiveHours={clock.ClockSpan}");
            List<ClockItemBaseDTO> clockItems = clocksService.GetClockItems(clock.ClockId).ToList();
            var regularClockItems = clockItems.Where(ci => ci.OrderIndex >= 0).ToList();

            //Contain events and sub-items for events that should be played at a specific time
            var specialClockItems = clockItems.Where(ci => ci.OrderIndex < 0).ToList();
            ShowClockItems(regularClockItems, specialClockItems);

            
            Dictionary<TimeSpan, ClockItemEventDTO?> eventsByHour = specialClockItems
                .Where(ci => !ci.ClockItemEventId.HasValue)
                .Select(ci => ci as ClockItemEventDTO)
                .ToDictionary(ci => ci!.EstimatedEventStart, ci => ci);


            int h = 0;
            for (int i = 1; i <= clock.ClockSpan; i++)
            {
                Console.WriteLine($"Generating for hour {h++}");
                foreach (ClockItemBaseDTO clockItem in regularClockItems)
                {
                    Console.WriteLine($"Id={clockItem.Id},OrderIndex={clockItem.OrderIndex}");
                    ProcessClockItem(clockItem, playlist, eventsByHour, specialClockItems);
                }
            }
        }

        private void ProcessClockItem(ClockItemBaseDTO clockItem,PlaylistDTO playlistDTO,
                                      IDictionary<TimeSpan, ClockItemEventDTO?> eventsByHour,
                                      ICollection<ClockItemBaseDTO> specialClockItems)
        {
            if (clockItem is ClockItemCategoryDTO itemCategory)
            {
                Console.WriteLine("Item is Category");
                if (itemCategory.CategoryId.HasValue)
                {
                    TrackSelectionBaseStrategy selectionStrategy;
                    if(itemCategory.ArtistSeparation != null && itemCategory.TitleSeparation != null && itemCategory.TrackSeparation != null)
                    {
                        selectionStrategy = new RandomTrackSelectionStrategy(playlistsService,
                                                                             tracksService,
                                                                             itemCategory.CategoryId.Value,
                                                                             itemCategory.ArtistSeparation.GetValueOrDefault(),
                                                                             itemCategory.TrackSeparation.GetValueOrDefault(),
                                                                             itemCategory.TitleSeparation.GetValueOrDefault());
                    }
                    else
                    {
                        selectionStrategy = new RandomTrackSelectionStrategy(playlistsService,
                                                                             tracksService,
                                                                             itemCategory.CategoryId.Value,
                                                                             artistSeparation: 30, //default separation
                                                                             trackSeparation: 10,
                                                                             titleSeparation: 30
                                                                             );
                    }
                    selectionStrategy.SelectTrack(playlistDTO);
                }
            }
            else if (clockItem is ClockItemTrackDTO itemTrack)
            {
                Console.WriteLine("Item is track");
            }
            

        }


        /// <summary>
        /// For debug in console
        /// </summary>
        /// <param name="clockItems"></param>
        private void ShowClockItems(ICollection<ClockItemBaseDTO> regularClockItems, ICollection<ClockItemBaseDTO> specialClockItems)
        {
            Console.WriteLine($"Current clock has {regularClockItems.Count()} regular items");

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
