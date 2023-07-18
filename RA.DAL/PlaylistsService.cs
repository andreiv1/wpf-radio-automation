using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using RA.Database;
using RA.Database.Models;
using RA.DTO;
using RA.DTO.Abstract;
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
            var nextDay = entity.AirDate.AddDays(1).Date;

            //if (playlistDTO.Items != null)
            //{
            //    for (int i = 0; i < playlistDTO.Items?.Count; i++)
            //    {
            //        var currentItem = playlistDTO.Items.ElementAt(i);
            //        var nextItem = playlistDTO.Items.ElementAtOrDefault(i + 1);

            //        //weird logic, but it works
            //        if (currentItem.ETA > nextDay) break;
            //        if (currentItem.Label != null && nextItem != null 
            //            && nextItem.ParentPlaylistItem == currentItem)
            //        {
            //            var parent = PlaylistItemDTO.ToEntity(currentItem);
            //            entity.PlaylistItems.Add(parent);

            //            int index = i + 1;
            //            int childrenNo = 1;
            //            while(playlistDTO.Items?.ElementAtOrDefault(index++)?.ParentPlaylistItem == currentItem)
            //            {
            //                var child = PlaylistItemDTO.ToEntity(playlistDTO.Items.ElementAt(index));
            //                if(child.Track != null)
            //                    child.Track = dbContext.AttachOrGetTrackedEntity(child.Track);
            //                child.ParentPlaylistItem = parent;
            //                entity.PlaylistItems.Add(child);
            //                childrenNo++;
            //            }
            //        } else
            //        {
            //            var playlistItem = PlaylistItemDTO.ToEntity(currentItem);
            //            if(playlistItem.Track != null)
            //                playlistItem.Track = dbContext.AttachOrGetTrackedEntity(playlistItem.Track);
            //           entity.PlaylistItems.Add(playlistItem);
            //        }
            //    }
            //}
            foreach(var item in entity.PlaylistItems)
            {
                if(item.Track != null)
                    item.Track = dbContext.AttachOrGetTrackedEntity(item.Track);
            }
            
            dbContext.Playlists.Add(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<PlaylistListingDTO>> GetPlaylistsToAirAfterDate(DateTime? date = null)
        {
            if (date == null) date = DateTime.Now.Date;
            using var dbContext = dbContextFactory.CreateDbContext();

            var query = await dbContext.Playlists.Where(p => p.AirDate >= date)
                .OrderBy(p => p.AirDate)
                .Select(p => PlaylistListingDTO.FromEntity(p))
                .ToListAsync();

            return query;
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

        public async Task<IEnumerable<PlaylistItemDTO>> GetPlaylistItems(int playlistId)
        {
            using var dbContext = dbContextFactory.CreateDbContext();

            var query = await dbContext.PlaylistItems
                .Include(pi => pi.Track)
                .ThenInclude(pi => pi.TrackArtists)
                .ThenInclude(ta => ta.Artist)
                .Where(pi => pi.PlaylistId == playlistId)
                .OrderBy(pi => pi.ETA)
                .Select(pi => PlaylistItemDTO.FromEntity(pi))
                .ToListAsync();

            return query;
        }

        public IEnumerable<PlaylistItemDTO> GetPlaylistItemsByDateTime(DateTime dateTimeStart, int maxHours = 1)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var activePlaylist = GetPlaylistsToAirAfterDate(dateTimeStart.Date).Result.FirstOrDefault();
            var pl = GetPlaylistsToAirAfterDate(dateTimeStart).Result.ToList();
            if(activePlaylist != null)
            {
                dateTimeStart = new DateTime(dateTimeStart.Year, dateTimeStart.Month, dateTimeStart.Day, dateTimeStart.Hour,dateTimeStart.Minute,0);
                DateTime dateTimeEnd = dateTimeStart.AddHours(maxHours);

                var query = dbContext.PlaylistItems
                     .Include(pi => pi.Track)
                     .ThenInclude(pi => pi.TrackArtists)
                     .ThenInclude(ta => ta.Artist)
                     .Where(pi => pi.PlaylistId == activePlaylist.Id)
                     .Where(pi => pi.ETA >= dateTimeStart)
                     .Where(pi => pi.ETA <= dateTimeEnd)
                     .Select(pi => PlaylistItemDTO.FromEntity(pi));
                foreach(var item in query)
                {
                    yield return item;
                }
            } 
        }

        public async Task<bool> PlaylistExists(DateTime date)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var query = await dbContext.Playlists.AnyAsync(p => p.AirDate == date);
            return query;
        }

        public async Task DeletePlaylist(int id)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            await dbContext.Playlists
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task AddPlaylistItem(PlaylistItemDTO playlistItemDTO)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var entity = PlaylistItemDTO.ToEntity(playlistItemDTO);

            var lastItem = await dbContext.PlaylistItems
                .Where(pi => pi.PlaylistId == entity.PlaylistId)
                .OrderByDescending(pi => pi.ETA)
                .FirstOrDefaultAsync();
            if (lastItem != null)
            {
                entity.ETA = lastItem.ETA.AddSeconds(lastItem.Length);
                entity.Track = dbContext.AttachOrGetTrackedEntity(entity.Track);
                entity.Length = entity.Track.Duration;
                dbContext.PlaylistItems.Add(entity);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeletePlaylistItem(int id)
        {
            using var dbContext = dbContextFactory.CreateDbContext();  
            await dbContext.PlaylistItems
                .Where(pi => pi.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}
