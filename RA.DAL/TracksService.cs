using Microsoft.EntityFrameworkCore;
using RA.DAL;
using RA.Database;
using RA.Database.Models;
using RA.DTO;
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

        public async Task<TrackDto> GetTrack(int id)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            return await dbContext.GetTrackById(id)
                .Include(c => c.Categories)
                .Select(t => TrackDto.FromEntity(t))
                .FirstAsync();
        }

        public async Task<IEnumerable<TrackListDto>> GetTrackListByArtistAsync(int artistId, int skip, int take)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.GetTracks()
                .Skip(skip).Take(take)
                .Where(t => t.TrackArtists.Contains(new ArtistTrack
                {
                    ArtistId = artistId,
                    TrackId = t.Id,
                }))
                .Select(t => TrackListDto.FromEntity(t))
                .ToListAsync();
        }

        public IEnumerable<TrackListDto> GetTrackListByArtist(int artistId, int skip, int take)
        {
            return GetTrackListByArtistAsync(artistId, skip, take).Result;
        }

        public async Task<IEnumerable<TrackListDto>> GetTrackListByCategoryAsync(int categoryId, int skip, int take)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.GetTracks()
                .Skip(skip).Take(take)
                .Where(t => t.Categories.Contains(new Category()
                    {
                        Id = categoryId,
                    }))
                .Select(t => TrackListDto.FromEntity(t))
                .ToListAsync();
        }
    }
}
