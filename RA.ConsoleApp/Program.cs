using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RA.DAL;
using RA.Database;
using RA.Database.Models;
using RA.DTO;
using RA.Logic.PlanningLogic;

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
            TestPlaylistGenerator();
            Console.ReadLine();
        }

       static void TestPlaylistGenerator()
        {
            IPlaylistGenerator playlistGenerator = new PlaylistGenerator(
                new PlaylistsService(dbFactory),
                new TracksService(dbFactory),
                new ClocksService(dbFactory),
                new TemplatesService(dbFactory),
                new SchedulesService(new SchedulesDefaultService(dbFactory),
                new SchedulesPlannedService(dbFactory))
                );
                ;

            var playlist = playlistGenerator.GeneratePlaylistForDate(new DateTime(2023,6,9));
        }
        
    }
}