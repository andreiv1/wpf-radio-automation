using Microsoft.Extensions.Hosting;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RA.Database;

namespace RA.UI.StationManagement.HostBuilders
{
    public static class AddDbContextHostBuilderExtensions
    {
        public static IHostBuilder AddDbContext(this IHostBuilder host)
        {
            host.ConfigureServices((context, services) =>
            {
                string? connectionString = context.Configuration.GetConnectionString("mariadb");
                if(connectionString == null) throw new Exception("Connection string is null");
                
                Action<DbContextOptionsBuilder> configureDbContext = o => o.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                services.AddDbContextFactory<AppDbContext>(configureDbContext);
            });

            return host;
        }
    }
}
