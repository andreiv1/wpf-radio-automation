using Microsoft.EntityFrameworkCore;
using RA.DAL.Interfaces;
using RA.Database;
using RA.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public class TracksService : ITracksService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;

        public TracksService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<int> GetTrackCountAsync()
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                return await dbContext.Tracks.CountAsync();
            }
        }

        public async Task<IEnumerable<TrackListDto>> GetTrackListAsync(int skip, int take)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                return await dbContext.GetTracks()
                    .Skip(skip)
                    .Take(take)
                    .Select(t => TrackListDto.FromEntity(t))
                    .ToListAsync();
            }
        }
    }
}
