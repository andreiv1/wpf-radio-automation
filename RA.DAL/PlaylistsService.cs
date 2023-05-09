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
    public class PlaylistsService : IPlaylistsService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;

        public PlaylistsService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task AddPlaylistAsync(PlaylistDTO playlistDTO)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var entity = PlaylistDTO.ToEntity(playlistDTO);
            foreach (var item in entity.PlaylistItems)
            {
                item.Track = dbContext.AttachOrGetTrackedEntity(item.Track);
            }
            dbContext.Playlists.Add(entity);
            await dbContext.SaveChangesAsync();
        }

        public IEnumerable<PlaylistListingDTO> GetPlaylistsToAir(DateTime? date = null)
        {
            if (date == null) date = DateTime.Now.Date;
            using var dbContext = dbContextFactory.CreateDbContext();
            var query = dbContext.Playlists.Where(p => p.AirDate >= date)
                .Select(p => PlaylistListingDTO.FromEntity(p));

            foreach (var item in query)
            {
                yield return item;
            }
        }

        public IEnumerable<PlaylistByHourDTO> GetPlaylistsByHour(DateTime airDate)
        {
            using var dbContext = dbContextFactory.CreateDbContext();

            var upperDateLimit = airDate.Date.AddDays(1);

            var playlistsByHour = dbContext.PlaylistItems
                .AsNoTracking()
                .Where(pi => pi.ETA >= airDate && pi.ETA <= upperDateLimit)
                .Select(h => new
                {
                    Hour = h.ETA.Hour,
                    Length = h.Length,
                    PlaylistId = h.PlaylistId
                })
               .GroupBy(h => new { h.Hour, h.PlaylistId })
               .Select(g => new PlaylistByHourDTO
               {
                   Hour = g.Key.Hour,
                   PlaylistId = g.Key.PlaylistId,
                   Length = g.Sum(h => h.Length)
               })
              .OrderBy(t => t.Hour)
              .ThenBy(t => t.PlaylistId);

            foreach (var item in playlistsByHour)
            {
                yield return item;
            }

        }
    }
}
