﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using RA.Database;
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
            foreach (var item in entity.PlaylistItems)
            {
                item.Track = dbContext.AttachOrGetTrackedEntity(item.Track);
            }
            dbContext.Playlists.Add(entity);
            await dbContext.SaveChangesAsync();
        }

        public IEnumerable<PlaylistListingDTO> GetPlaylistsToAirAfterDate(DateTime? date = null)
        {
            if (date == null) date = DateTime.Now.Date;
            using var dbContext = dbContextFactory.CreateDbContext();
            var query = dbContext.Playlists.Where(p => p.AirDate >= date)
                .OrderBy(p => p.AirDate)
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

        public IEnumerable<PlaylistItemDTO> GetPlaylistItems(int playlistId)
        {
            using var dbContext = dbContextFactory.CreateDbContext();

            var query = dbContext.PlaylistItems
                .Include(pi => pi.Track)
                .ThenInclude(pi => pi.TrackArtists)
                .ThenInclude(ta => ta.Artist)
                .Where(pi => pi.PlaylistId == playlistId)
                .Select(pi => PlaylistItemDTO.FromEntity(pi));

            foreach(var item in query)
            {
                yield return item;
            }
        }

        public IEnumerable<PlaylistItemDTO> GetPlaylistItemsByDateTime(DateTime dateTimeStart, int maxHours = 1)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var activePlaylist = GetPlaylistsToAirAfterDate(dateTimeStart.Date).FirstOrDefault();
            var pl = GetPlaylistsToAirAfterDate(dateTimeStart).ToList();
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


    }
}
