using Microsoft.EntityFrameworkCore;
using RA.DAL;
using RA.DAL.Interfaces;
using RA.Database.Models;
using RA.Logic.Planning;
using System.Text.RegularExpressions;

namespace RA.ConsoleApp
{
    public class Program
    {
        static DbContextFactory dbFactory = new DbContextFactory();
        static void Main(string[] args)
        {
            TestPlaylistGenerator();
            Console.ReadLine();
        }


        static async void TestPlaylistGenerator()
        {
            var playlistGen = new PlaylistGenerator(dbFactory,
                                                    new SchedulesService(new SchedulesDefaultService(dbFactory), new SchedulesPlannedService(dbFactory)),
                                                    new ClocksService(dbFactory),
                                                    new TemplatesService(dbFactory));
            PlaylistGenerator.DefaultArtistSeparation = 30;
            PlaylistGenerator.DefaultTitleSeparation = 30;
            PlaylistGenerator.DefaultTrackSeparation = 60;
            await playlistGen.GeneratePlaylistForDate(DateTime.Now.Date);
        }
        static void TestAllSongsIncat()
        {
            var db = dbFactory.CreateDbContext();
            var tracks = db.GetTracksByCategoryId(1)
                .Include(t => t.TrackArtists)
                .ThenInclude(t => t.Artist);
            foreach(var t in tracks)
            {
                Console.WriteLine($"{t.Id} - {t.TrackArtists.Count}");
            }
        }

        static async Task TestScheduleService()
        {
            SchedulesDefaultService defaultService = new SchedulesDefaultService(dbFactory);
            SchedulesPlannedService plannedService = new SchedulesPlannedService(dbFactory);
            SchedulesService schedulesService = new SchedulesService(defaultService, plannedService);
            var result = await schedulesService.GetSchedulesOverview(new DateTime(2023, 7, 1), new DateTime(2023, 7, 31));
        }
        static async Task TestPlannedOverview()
        {
            SchedulesPlannedService plannedService = new SchedulesPlannedService(dbFactory);
            var s = await plannedService.GetPlannedSchedulesOverviewAsync(new DateTime(2023, 7, 1), new DateTime(2023, 7, 31));
            var first = s.First();
            Console.WriteLine($"{first.Value.Name} - from {first.Value.StartDate.Value.ToString("dd.MM.yyyy - dddd")} to {first.Value.EndDate.Value.ToString("dd.MM.yyyy - dddd")}");
            Console.WriteLine($"Freq: {first.Value.Frequency.ToString()}");
            Console.WriteLine($"Days:\n" +
                $"Lu {GetYesOrNo(first.Value.IsMonday.GetValueOrDefault())}");
            Console.WriteLine($"Ma {GetYesOrNo(first.Value.IsTuesday.GetValueOrDefault())}");
            Console.WriteLine($"Mi {GetYesOrNo(first.Value.IsWednesday.GetValueOrDefault())}");
            Console.WriteLine($"Jo {GetYesOrNo(first.Value.IsThursday.GetValueOrDefault())}");
            Console.WriteLine($"Vi {GetYesOrNo(first.Value.IsFriday.GetValueOrDefault())}");
            Console.WriteLine($"Sa {GetYesOrNo(first.Value.IsSaturday.GetValueOrDefault())}");
            Console.WriteLine($"Du {GetYesOrNo(first.Value.IsSunday.GetValueOrDefault())}");

            foreach(var x in s)
            {
                Console.WriteLine($"{x.Key.ToString("dd.MM.yyyy - dddd")}");
            }
        }
        static async Task TestHistory()
        {
            TrackHistoryService trackHistoryService = new TrackHistoryService(dbFactory);
            await trackHistoryService.GetMostPlayedTracks(DateTime.Now.Date.AddDays(-1),DateTime.Now.Date.AddDays(1));
        }

        static string GetYesOrNo(bool value)
        {
            return value ? "yes" : "no";
        }
        static async Task MusicCategoryDistribution()
        {

        }
    }
}