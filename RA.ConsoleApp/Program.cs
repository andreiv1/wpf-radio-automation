using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RA.DAL;
using RA.DAL.Interfaces;
using RA.Database;
using RA.Database.Models;
using RA.DTO;
using RA.Logic.Planning;

namespace RA.ConsoleApp
{
    public class DbContextFactory : IDbContextFactory<AppDbContext>
    {
        private static String connString = "server=localhost;Port=3306;database=raprod;user=root;password=";
        public AppDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
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
            IUsersService usersService = new UsersService(dbFactory);

            var result = usersService.AddUser(new UserDTO
            {
                Username = "andrei",
                Password = "andrei",
                FullName = "Andrei",
                GroupId = 1
            });

            result.Wait();

            Console.WriteLine($"CanAddUser={result.Result}");

            var result2 = usersService.CanUserLogIn("andrei", "andrei");

            Console.WriteLine($"CanUserLogIn={result2.Result}");


        }

      
        
    }
}