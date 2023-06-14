using Microsoft.EntityFrameworkCore;
using RA.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public class SchedulesPlannedService : ISchedulesPlannedService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;

        public SchedulesPlannedService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }
    }
}
