using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RA.DAL;
using RA.Database;

namespace RA.ConsoleApp
{
    public class DbContextFactory : IDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            String connString = "server=192.168.200.113;Port=3306;database=rasoftware;user=root;password=andrewyw1412";
            DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptions<AppDbContext>();

            dbContextOptions = optionsBuilder.UseMySql(connString, ServerVersion.AutoDetect(connString))
                .EnableSensitiveDataLogging(false)
                .Options;
            return new AppDbContext(dbContextOptions);
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            TestDefaultScheduleOverview();
        }

        static void TestDefaultScheduleOverview()
        {
            var dbFactory = new DbContextFactory();
            //var db = dbFactory.CreateDbContext();
            var dsService = new DefaultScheduleService(dbFactory);
            var schedulesDates = dsService.GetDefaultScheduleOverview(DateTime.Now.Date,DateTime.Now.Date.AddDays(20));
            foreach(var s in schedulesDates)
            {
                Console.WriteLine($"{s.Value} - {s.Key}");
            }
        }
        
    }
}