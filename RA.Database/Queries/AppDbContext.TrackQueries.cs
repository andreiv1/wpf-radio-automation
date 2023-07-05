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

            var searchTerms = query.Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries);

            if (searchTerms.Any())
            {
                // Starting with all tracks
                result = Tracks.Include(t => t.TrackArtists).ThenInclude(t => t.Artist);

                foreach (var term in searchTerms)
                {
                    var lowerTerm = term.ToLower();

                    result = result.Where(t => t.Title.ToLower().Contains(lowerTerm) ||
                                         t.TrackArtists.Any(a => a.Artist.Name.ToLower().Contains(lowerTerm)));
                }
            }
            else
            {
                result = Tracks.Include(t => t.TrackArtists).ThenInclude(t => t.Artist);
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
                .Where(t => t.Id.Equals(id))
                .AsNoTracking();
        }
    }
}
