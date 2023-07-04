using Microsoft.EntityFrameworkCore;
using RA.Database;

namespace RA.DAL
{
    public class ReportsService : IReportsService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;

        public ReportsService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }
    }
}
