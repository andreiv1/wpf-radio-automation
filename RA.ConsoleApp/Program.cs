using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RA.DAL;
using RA.Database;
using RA.Database.Models;
using RA.DTO;
using RA.Logic.PlanningLogic;
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
            IClocksService clocksService = new ClocksService(dbFactory);

            clocksService.AddClockItem(new ClockItemDTO()
            {
                OrderIndex = 23,
                EventType = Database.Models.Enums.EventType.Marker,
                EstimatedEventDuration = new TimeSpan(0,2,0),
                EventLabel = "DJ Talk",
                ClockId = 1
            });

            Console.ReadKey();
        }
        
    }
}