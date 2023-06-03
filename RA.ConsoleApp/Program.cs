using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RA.DAL;
using RA.Database;
using RA.Database.Models;
using RA.DTO;


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
            //TestTracks();
            TestClocks();
            Console.ReadLine();
        }

        static async void TestTracks()
        {
            ICategoriesService categoriesService = new CategoriesService(dbFactory);
            var result = await categoriesService.NoOfTracksMatchingConditions(1);
            Console.WriteLine(result);
            Console.ReadKey();
        }

        static async void TestClocks()
        {
            IClocksService clService = new ClocksService(dbFactory);
            var result = await clService.GetClockItemAsync(62);
        }
        
    }
}