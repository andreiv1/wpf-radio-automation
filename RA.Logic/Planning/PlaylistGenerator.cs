using Microsoft.EntityFrameworkCore;
using RA.DAL;
using RA.Database;
using RA.Database.Models;
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
        private static TimeSpan clockMinLength = new TimeSpan(1, 0, 0);

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


            if (regularClockItems.Count == 0)
            {
                var emptyClock = clocksService.GetClock(clock.ClockId).Result;
                var start = clock.StartTime.ToString(@"hh\:mm");
                var end = clock.StartTime.Add(TimeSpan.FromHours(clock.ClockSpan)).ToString(@"hh\:mm");
                throw new PlaylistException($"This template has a clock '{emptyClock.Name}' without any items;\n" +
                    $"it starts at {start} and it ends at {end}");
            }

            int h = 0;
            
            for (int i = 1; i <= clock.ClockSpan; i++)
            {
                DebugHelper.WriteLine(this,$"Generating for hour {h++}");
                TimeSpan clockDuration = TimeSpan.Zero;
                foreach (ClockItemBaseDTO clockItem in regularClockItems)
                {
                    DebugHelper.WriteLine(this, $"Id={clockItem.Id},OrderIndex={clockItem.OrderIndex}");
                   
                    ProcessClockItem(clockItem, playlist, eventsByHour, specialClockItems, ref clockDuration);
                }

                if (clockDuration < clockMinLength && !clockItems.OfType<ClockItemCategoryDTO>().Any() 
                    && clockItems.Cast<ClockItemCategoryDTO?>().Any(c => c?.IsFiller ?? false))
                {
                    var relatedClock = clocksService.GetClock(clock.ClockId).Result;
                    throw new PlaylistException($"The clock '{relatedClock.Name}' duration is not an hour or there isn't any filler!");
                } else
                {
                    //Fillers exist
                    //throw new PlaylistException($"Fillers exist"); 
                    var fillers = clockItems.OfType<ClockItemCategoryDTO>().Where(c => c.IsFiller).ToList();

                    for(int f = 0; i < fillers.Count && clockDuration < clockMinLength; f++)
                    {
                        ProcessClockItem(fillers[f], playlist, eventsByHour, specialClockItems, ref clockDuration);

                        //If clock duration does not exceds the min length, readd fillers
                        if (clockDuration < clockMinLength && f == fillers.Count - 1)
                        {
                            f = 0;
                        }
                    }
                }
            }
        }

        private void ProcessClockItem(ClockItemBaseDTO clockItem, PlaylistDTO playlist,
                                      IDictionary<TimeSpan, ClockItemEventDTO?> eventsByHour,
                                      ICollection<ClockItemBaseDTO> specialClockItems,
                                      ref TimeSpan clockDuration)
        {
            PlaylistItemDTO? lastItem = playlist.Items?.LastOrDefault();
            PlaylistItemDTO? selectedItem = null;

            if (clockItem is ClockItemCategoryDTO itemCategory)
            {
                DebugHelper.WriteLine(this, "*** Processing category ***");
                TrackSelectionOptions options = new()
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
                selectedItem = selectionStrategy.SelectTrack(playlist);
            } 
            else if(clockItem is ClockItemTrackDTO itemTrack)
            {
                DebugHelper.WriteLine(this, "*** Processing track ***");
                selectedItem = new PlaylistItemDTO
                {
                    Length = itemTrack.TrackDuration.TotalSeconds,
                    Track = new TrackListingDTO()
                    {
                        Id = itemTrack.TrackId,
                        Artists = itemTrack.Artists,
                        Title = itemTrack.Title,
                    }
                };
            }

            var eventFound = ProcessEvents(playlist, eventsByHour, specialClockItems, ref clockDuration);
            // Check if there is any close event
          
            if (eventFound)
            {
                if(lastItem != null && selectedItem != null)
                {
                    selectedItem.ETA = lastItem.ETA.AddSeconds(selectedItem.Length);
                } else
                {
                    if (selectedItem == null) throw new PlaylistException($"Playlist failed because there was an error when selecting an item.");
                    else
                    {
                        selectedItem.ETA = new DateTime(playlist.AirDate.Year, playlist.AirDate.Month, playlist.AirDate.Day);
                    }
                    
                }
            }

            clockDuration = clockDuration + TimeSpan.FromSeconds(selectedItem?.Length ?? 0);
            playlist.Items?.Add(selectedItem);
        }

        private bool ProcessEvents(PlaylistDTO playlist, 
                                   IDictionary<TimeSpan, ClockItemEventDTO?> eventsByHour, 
                                   ICollection<ClockItemBaseDTO> specialClockItems,
                                   ref TimeSpan clockDuration)
        {
            bool eventFound = false;
            PlaylistItemDTO? lastItem = playlist.Items?.LastOrDefault();
            PlaylistItemDTO? selectedItem = null;
            if (lastItem != null && selectedItem != null)
            {
                TimeSpan lastItemTime = new TimeSpan(0, lastItem.ETA.Minute, lastItem.ETA.Second);
                TimeSpan selectedItemTime = new TimeSpan(0, selectedItem.ETA.Minute, selectedItem.ETA.Second);

                DateTime lastDateTime = selectedItem.ETA;

                foreach (var kvp in eventsByHour)
                {
                    if (kvp.Key > lastItemTime && kvp.Key <= selectedItemTime && kvp.Value != null)
                    {
                        DebugHelper.WriteLine(this, $"Close event found at time: {kvp.Key}; added at {lastDateTime}");
                        eventFound = true;
                        var parentEvent = new PlaylistItemDTO
                        {
                            ETA = lastDateTime,
                            EventType = kvp.Value.EventType,
                            Label = kvp.Value.EventLabel,
                        };
                        playlist.Items?.Add(parentEvent);
                        var eventItems = specialClockItems.Where(ci => ci.ClockItemEventId == kvp.Value.Id)
                            .OrderBy(ci => ci.EventOrderIndex).ToList();

                        foreach (var eventItem in eventItems)
                        {
                            DebugHelper.WriteLine(this, "Adding event item: " + eventItem.Id);
                            if (eventItem is ClockItemTrackDTO itemEventTrack)
                            {
                                playlist.Items?.Add(new PlaylistItemDTO
                                {
                                    ETA = lastDateTime,
                                    Length = itemEventTrack.TrackDuration.TotalSeconds,
                                    Track = new TrackListingDTO()
                                    {
                                        Id = itemEventTrack.TrackId,
                                        Artists = itemEventTrack.Artists,
                                        Title = itemEventTrack.Title,
                                    },
                                    ParentPlaylistItem = parentEvent,
                                });
                            }
                            else if (eventItem is ClockItemCategoryDTO itemEventCategory)
                            {
                                TrackSelectionOptions optionsEventItem = new()
                                {
                                    ArtistSeparation = 0, //Ignore artist sep for event items
                                    TrackSeparation = itemEventCategory.TrackSeparation.HasValue ? itemEventCategory.TrackSeparation : DefaultTrackSeparation,
                                    TitleSeparation = itemEventCategory.TitleSeparation.HasValue ? itemEventCategory.TitleSeparation : DefaultTitleSeparation,
                                    MinDuration = itemEventCategory.MinDuration,
                                    MaxDuration = itemEventCategory.MaxDuration,
                                    MinReleaseDate = itemEventCategory.MinReleaseDate,
                                    MaxReleaseDate = itemEventCategory.MaxReleaseDate,
                                    TagValuesIds = itemEventCategory.Tags?.Count > 0 ? itemEventCategory.Tags.Select(t => t.TagValueId).ToList() : null,
                                };

                                RandomTrackSelectionStrategy selectionEventStrategy = new RandomTrackSelectionStrategy(dbContextFactory, optionsEventItem,
                                    itemEventCategory.CategoryId.GetValueOrDefault());
                                var selectedByEvent = selectionEventStrategy.SelectTrack(playlist);
                                selectedByEvent.ParentPlaylistItem = parentEvent;
                                playlist.Items?.Add(selectedByEvent);
                            }

                            //??
                            var lastItemAdded = playlist.Items?.LastOrDefault();
                            if (lastItemAdded != null)
                            {
                                lastDateTime = lastItemAdded.ETA.AddSeconds(lastItemAdded.Length);
                            }
                        }
                        //Event found and filled, no need to continue  
                        break;
                    }
                }
            }
            return eventFound;
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

        //private DateTime? GetLastPlaylist
    }
}
