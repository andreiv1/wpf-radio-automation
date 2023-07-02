using Microsoft.EntityFrameworkCore;
using RA.Database;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public class TrackHistoryService : ITrackHistoryService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;

        public TrackHistoryService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task AddTrackToHistory(TrackHistoryDTO trackHistoryDTO)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var entity = TrackHistoryDTO.ToEntity(trackHistoryDTO);
            dbContext.TrackHistory.Add(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}
