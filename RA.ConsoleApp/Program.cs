using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RA.DAL;
using RA.Database;
using RA.Database.Models;
using RA.Logic.TrackFileLogic;
using RA.Logic.TrackFileLogic.Models;

namespace RA.ConsoleApp
{
    public class DbContextFactory : IDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            String connString = "server=localhost;Port=3306;database=ratest;user=root;password=";
            DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptions<AppDbContext>();

            dbContextOptions = optionsBuilder.UseMySql(connString, ServerVersion.AutoDetect(connString))
                .EnableSensitiveDataLogging(false)
                .Options;
            return new AppDbContext(dbContextOptions);
        }
    }
    public class Program
    {
        static DbContextFactory dbFactory = new DbContextFactory();
        static void Main(string[] args)
        {
            TestImportDirectory();
            Console.ReadLine();
        }

        static async void TestImport()
        {
            var db = dbFactory.CreateDbContext();
            IArtistsService artistsService = new ArtistsService(dbFactory);
            ITrackFilesProcessor processor = new TrackFilesProcessor(artistsService);
            TrackMetadataReader.ImagePath = "C:\\Users\\Andrei\\Desktop\\images";
            var track = processor.ProcessSingleItem(
                @"C:\Users\Andrei\Music\FakeRadio\Music\Current Hits\Miley Cyrus - River.mp3", true);
            
            ITracksService tracksService = new TracksService(dbFactory);
            ITrackFilesImporter importer = new TrackFileImporter(tracksService);
            await importer.ImportAsync(new ProcessingTrack[] { track });
            Console.WriteLine($"Imported");
        }

        static async void TestImportDirectory()
        {
            var db = dbFactory.CreateDbContext();
            IArtistsService artistsService = new ArtistsService(dbFactory);
            ITracksService tracksService = new TracksService(dbFactory);
            ITrackFilesProcessor processor = new TrackFilesProcessor(artistsService);
            TrackMetadataReader.ImagePath = "C:\\Users\\Andrei\\Desktop\\images";
            TrackFilesProcessorOptionsBuilder optionsBuilder = new TrackFilesProcessorOptionsBuilder(
                @"C:\Users\Andrei\Music\FakeRadio\Music\Current Hits", 1);

            var tracks = processor.ProcessItemsFromDirectory(optionsBuilder
                .SetReadMetadata(true)
                .Build()).ToList();

            ITrackFilesImporter importer = new TrackFileImporter(tracksService);
            await importer.ImportAsync(tracks);

        }
        static void CreateDefaultSchedule()
        {
            var db = dbFactory.CreateDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            //Creating dummy templates
            List<Template> templates = new List<Template>();
            for(int i = 0; i < 5; i++)
            {
                templates.Add(new Template()
                {
                    Name = $"Dummy template {i}"
                });
            }

            List<ScheduleDefault> schedules = new List<ScheduleDefault>();
            DateTime startDate = DateTime.Today.AddDays(-1);
            DateTime endDate = startDate.AddDays(7);
            var random = new Random();

            //Creating 10 schedules
            for(int i = 1; i <= 10; i++)
            {
                var s = new ScheduleDefault();
                s.StartDate = startDate;
                s.EndDate = endDate;

                schedules.Add(s);

                //Adding items

                s.ScheduleDefaultItems = new List<ScheduleDefaultItem>();
                for (DayOfWeek day = DayOfWeek.Sunday; day <= DayOfWeek.Saturday; day++)
                {

                    s.ScheduleDefaultItems.Add(
                    new ScheduleDefaultItem()
                    {
                        Schedule = s,
                        DayOfWeek = day,
                        Template = templates.ElementAt(random.Next(1, 5)),

                    });
                }

                // Move to the next week
                startDate = endDate.AddDays(1);
                endDate = startDate.AddDays(7);

            }
            db.SchedulesDefault.AddRange(schedules);
            db.SaveChanges();

            foreach(var s in db.SchedulesDefault
                .Include(s => s.ScheduleDefaultItems)
                .ThenInclude(itm => itm.Template))
            {
                Console.WriteLine($"DEFAULT SCHEDULE BETWEEN {s.StartDate} - {s.EndDate}");
                foreach (var itm in s.ScheduleDefaultItems)
                {
                    Console.WriteLine($"  >>> {(DayOfWeek)itm.DayOfWeek} - {itm.Template.Name}");
                }
            }
        }
        
    }
}