using Microsoft.EntityFrameworkCore;
using RA.Database;
using RA.DTO;
using RA.DTO.Abstract;
using RA.Logic.PlanningLogic.Interfaces;
using RA.Logic.PlanningLogic.Old;
using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.PlanningLogic
{
    public class PlaylistGenerator : IPlaylistGenerator
    {
        private readonly IScheduleManager scheduleManager;

        public PlaylistGenerator(IScheduleManager scheduleManager)
        {
            this.scheduleManager = scheduleManager;
        }

        private AppDbContext db;
        public Playlist GeneratePlaylist(AppDbContext db, DateTime date)
        {
            this.db = db;
            Playlist playlist = InitializePlaylist(date);
            DateTime estimatedPlaylistStart = new DateTime(date.Year, date.Month, date.Day);
            try
            {
                var schedule = scheduleManager.GetDefaultSchedule(date);

                int scheduleTemplateId = schedule.TemplateDto?.Id ?? throw new Exception("Template must have an id");

                var clocksForSchedule = GetClocksByTemplate(scheduleTemplateId);

                foreach (var clock in clocksForSchedule)
                {
                    ProcessClock(clock, ref estimatedPlaylistStart, playlist);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Cannot generate playlist for date {date}.\n {ex.Message}");
            }

            return playlist;
        }

        private void ProcessClock(ClockTemplateDto clock, ref DateTime estimatedPlaylistStart, Playlist playlistDto)
        {
            TimeSpan clockSpan = new TimeSpan(clock.ClockSpan, 0, 0);
            TimeSpan clockStart = clock.StartTime;
            TimeSpan clockEnd = clockStart.Add(clockSpan);

            Console.WriteLine($"ClockId={clock.ClockId},ClockStart={clockStart},ClockEnd={clockEnd}");
            Console.WriteLine($"Generating playlist for {clock.ClockSpan} consecutive hours");

            List<ClockItem> clockItems;

            clockItems = db.ClockItems
                .Where(ci => ci.ClockId == clock.ClockId)
                .ToList();

            Console.WriteLine($"Current clock has {clockItems.Count()} items");


            int h = 0;
            for (int i = 1; i <= clock.ClockSpan; i++)
            {
                Console.WriteLine($"Generating for hour {h++}");

                foreach (ClockItem clockItem in clockItems)
                {
                    ProcessClockItem(clockItem, h, ref estimatedPlaylistStart, playlistDto);
                }

                Console.WriteLine("=======================================");
            }
        }

        Random random = new Random();
        private void ProcessClockItem(ClockItem clockItem, int h, ref DateTime estimatedPlaylistStart, Playlist playlist)
        {
            Console.WriteLine($"Processing clock item {clockItem.Id}");

            if (clockItem.CategoryId.HasValue)
            {
                Track nextTrack;
                Console.WriteLine("Selection from category");

                var categoryQuery = db.Categories
                    .Include(c => c.Tracks)
                    .Where(c => c.Id == clockItem.CategoryId);
                var categoryTracks = categoryQuery.First().Tracks;
                int count = categoryTracks.Count();

                //TODO - random strategy
                int randomIndex = random.Next(0, count);
                nextTrack = categoryTracks.ElementAt(randomIndex);

                PlaylistItem playlistItem = new();
                playlistItem.Playlist = playlist;
                playlistItem.Track = nextTrack;
                playlistItem.Length = nextTrack.Duration;
                playlistItem.ETA = estimatedPlaylistStart;

                // Get the current hour of estimatedPlaylistStart
                int currentHour = estimatedPlaylistStart.Hour;

                var nextDay = new DateTime(estimatedPlaylistStart.Year, estimatedPlaylistStart.Month, estimatedPlaylistStart.Day + 1);
                estimatedPlaylistStart += TimeSpan.FromSeconds(playlistItem.Length);

                if(estimatedPlaylistStart >= nextDay)
                {
                    //Temp-Do not allow the playlist to move to the next day
                    return;
                }
                
                Console.WriteLine($"{playlistItem.ETA} - {playlistItem.Track.Title}");

                playlist.PlaylistItems.Add(playlistItem);

            }

        }

        private Playlist InitializePlaylist(DateTime date)
        {
            Playlist playlist = new Playlist();
            playlist.AirDate = date;
            playlist.DateAdded = DateTime.Now;
            playlist.PlaylistItems = new List<PlaylistItem>();
            return playlist;
        }

        public List<ClockTemplateDto> GetClocksByTemplate(int templateId)
        {

            return db.ClockTemplates
                .Where(ct => ct.TemplateId == templateId)
                .OrderBy(ct => ct.StartTime)
                .Select(ct => ClockTemplateDto.FromEntity(ct))
                .ToList();

        }
    }
}
