using Microsoft.EntityFrameworkCore;
using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database
{
    public partial class AppDbContext : DbContext
    {
        /// <summary>
        /// Get all tracks including artists
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQueryable<Track> GetTracks(String query = "")
        {
            query = query.Trim();
            IQueryable<Track> result;
            if(query != String.Empty)
            {
                result = Tracks.Include(t => t.TrackArtists)
                         .ThenInclude(t => t.Artist)
                         .Where(t => t.Title.ToLower().StartsWith(query) ||
                             t.TrackArtists.Any(a => a.Artist.Name.ToLower().Contains(query)) || 
                             t.FilePath.ToLower().Contains(query)
                         );
            } else
            {
                result = Tracks.Include(t => t.TrackArtists)
                         .ThenInclude(t => t.Artist);
            }

            return result;
        }

        /// <summary>
        /// Get track with artists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IQueryable<Track> GetTrackById(int id)
        {
            return Tracks.Include(t => t.TrackArtists)
                .ThenInclude(t => t.Artist)
                .Where(t => t.Id.Equals(id));
        }
    }
}
