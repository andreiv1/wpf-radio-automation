using Microsoft.EntityFrameworkCore;
using RA.DAL;
using RA.Database;
using RA.DTO;
using RA.DTO.Abstract;
using RA.Logic.Planning.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.Planning
{
    public class PlaylistException : Exception
    {
        public PlaylistException(string? message) : base(message)
        {
        }
    }
    public class PlaylistGenerator : IPlaylistGenerator
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;
        private readonly ISchedulesService schedulesService;
        private readonly IClocksService clocksService;
        private readonly ITemplatesService templatesService;

        public static int DefaultArtistSeparation { get; set; }
        public static int DefaultTrackSeparation { get; set; }
        public static int DefaultTitleSeparation { get; set; }

        public PlaylistGenerator(IDbContextFactory<AppDbContext> dbContextFactory,
                                 ISchedulesService schedulesService,
                                 IClocksService clocksService,
                                 ITemplatesService templatesService)
        {
            this.dbContextFactory = dbContextFactory;
            this.schedulesService = schedulesService;
            this.clocksService = clocksService;
            this.templatesService = templatesService;
        }

        public async Task<PlaylistDTO> GeneratePlaylistForDate(DateTime date)
        {
            PlaylistDTO playlist = PlaylistDTO.Initialise(date);
            IScheduleDTO? schedule = await schedulesService.GetScheduleByDate(date);
            if (schedule != null)
            {
                int scheduleTemplateId = schedule.Template?.Id ?? throw new PlaylistException("Template must have an id.");
                var clocksForSchedule = templatesService.GetClocksForTemplate(scheduleTemplateId).ToList();
                bool isTemplateFilled = ValidateClocks(clocksForSchedule);
                if(!isTemplateFilled) throw new PlaylistException($"Template {schedule.Template.Name} is not filled entirely.");
                Console.WriteLine($"Trying to generate playlist for {date.ToString("dd/mm/yyyy")} with template: {schedule.Template.Name}");

                foreach (var clock in clocksForSchedule)
                {
                    ProcessClock(clock, playlist);
                }
            }
            else
            {
                throw new PlaylistException($"Can't generate playlist for date {date.ToString("dd.MM.yyyy")} without any schedule");
            }
            return playlist;
        }

        //Validate if a template has clocks to fill the entire day
        private bool ValidateClocks(ICollection<ClockTemplateDTO> clockTemplates)
        {
            for(int i = 0; i < clockTemplates.Count - 1; i++)
            {
                var current = clockTemplates.ElementAt(i);
                var next = clockTemplates.ElementAt(i + 1);

                var currentEndTime = current.StartTime + TimeSpan.FromHours(current.ClockSpan);
                var nextStartTime = next.StartTime;

                if (currentEndTime != nextStartTime) return false;
            }

            var last = clockTemplates.LastOrDefault();
            if (last != null)
            {
                var lastEndTime = last.StartTime + TimeSpan.FromHours(last.ClockSpan);
                var endOfDay = TimeSpan.FromHours(24);
                if (lastEndTime != endOfDay) return false;
            } else
            {
                return false;
            }
            
            return true;
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
            //ShowClockItems(regularClockItems, specialClockItems);

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

        private void ProcessClockItem(ClockItemBaseDTO clockItem, PlaylistDTO playlistDTO,
                                      IDictionary<TimeSpan, ClockItemEventDTO?> eventsByHour,
                                      ICollection<ClockItemBaseDTO> specialClockItems)
        {
            if (clockItem is ClockItemCategoryDTO itemCategory)
            {
                Console.WriteLine("* Category");
                TrackSelectionOptions options = new TrackSelectionOptions
                {
                    ArtistSeparation = itemCategory.ArtistSeparation.HasValue ? itemCategory.ArtistSeparation : DefaultArtistSeparation,
                    TrackSeparation = itemCategory.TrackSeparation.HasValue ? itemCategory.TrackSeparation : DefaultTrackSeparation,
                    TitleSeparation = itemCategory.TitleSeparation.HasValue ? itemCategory.TitleSeparation : DefaultTitleSeparation,
                    MinDuration = itemCategory.MinDuration,
                    MaxDuration = itemCategory.MaxDuration,
                    MinReleaseDate = itemCategory.MinReleaseDate,
                    MaxReleaseDate = itemCategory.MaxReleaseDate,
                    TagValuesIds = itemCategory.Tags?.Count > 0 ? itemCategory.Tags.Select(t => t.TagValueId).ToList() : null,
                };
                RandomTrackSelectionStrategy selectionStrategy = new RandomTrackSelectionStrategy(dbContextFactory, options, itemCategory.CategoryId!.Value);
                var selectedItem = selectionStrategy.SelectTrack(playlistDTO);
                playlistDTO.Items?.Add(selectedItem);
            } else if (clockItem is ClockItemTrackDTO itemTrack)
            {
                Console.WriteLine("* Track");
            }
        }

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
