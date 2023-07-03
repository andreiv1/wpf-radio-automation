﻿using Microsoft.EntityFrameworkCore;
using RA.Database;
using RA.Database.Models.Enums;
using RA.DTO;

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

        public async Task<ICollection<TrackHistoryListingDTO>> RetrieveTrackHistory(DateTime dateMinPlayed)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.TrackHistory
                .Where(th => th.DatePlayed >= dateMinPlayed)
                .Include(th => th.Track)
                .Include(th => th.Track!.TrackArtists)
                .ThenInclude(ta => ta.Artist)
                .OrderByDescending(th => th.DatePlayed)
                .Select(th => TrackHistoryListingDTO.FromEntity(th))
                .ToListAsync();
            return result;
        }

        public async Task<ICollection<TrackHistoryListingDTO>> GetHistoryBetween(DateTime dateStart,
                                                                                 DateTime dateEnd,
                                                                                 IList<TrackType> trackTypes)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.TrackHistory
                .Where(th => th.DatePlayed >= dateStart && th.DatePlayed <= dateEnd)
                .Where(th => trackTypes.Contains(th.TrackType))
                .Include(th => th.Track)
                .Include(th => th.Track!.TrackArtists)
                .ThenInclude(ta => ta.Artist)
                .OrderBy(th => th.DatePlayed)
                .Select(th => TrackHistoryListingDTO.FromEntity(th))
                .ToListAsync();
            return result;
        }

        public async Task<TrackHistoryListingDTO?> RetrieveItem(DateTime datePlayed)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var result = await dbContext.TrackHistory
                .Where(th => th.DatePlayed == datePlayed)
                .Include(th => th.Track)
                .Include(th => th.Track!.TrackArtists)
                .ThenInclude(ta => ta.Artist)
                .OrderByDescending(th => th.DatePlayed)
                .Select(th => TrackHistoryListingDTO.FromEntity(th))
                .FirstOrDefaultAsync();

            return result;
        }


    }
}
