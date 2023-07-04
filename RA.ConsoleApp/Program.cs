using RA.DAL;

namespace RA.ConsoleApp
{
    public class Program
    {
        static DbContextFactory dbFactory = new DbContextFactory();
        static void Main(string[] args)
        {
            _ = TestHistory();

            Console.ReadKey();
            
        }

        static async Task TestHistory()
        {
            TrackHistoryService trackHistoryService = new TrackHistoryService(dbFactory);
            await trackHistoryService.GetMostPlayedTracks(DateTime.Now.Date.AddDays(-1),DateTime.Now.Date.AddDays(1));
        }

        static async Task MusicCategoryDistribution()
        {

        }
    }
}