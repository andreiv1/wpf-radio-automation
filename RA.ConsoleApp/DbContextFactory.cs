using Microsoft.EntityFrameworkCore;
using RA.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
