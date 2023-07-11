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
                .Where(t => t.Id.Equals(id));
        }

        /// <summary>
        /// Get all tracks for a category (including subcategories)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IQueryable<Track> GetTracksByCategoryId(int id)
        {
            string rawQuery = @"
                WITH RECURSIVE CategoryHierarchy AS (
                    SELECT Id
                    FROM Categories
                    WHERE Id = {0} -- Specify the starting CategoryId
                    UNION ALL
                    SELECT c.Id
                    FROM Categories c
                    INNER JOIN CategoryHierarchy ch ON c.ParentId = ch.Id
                )
        
                SELECT t.*, c.Id AS CategoryId, c.Name AS CategoryName
                FROM Tracks t
                JOIN categories_tracks ct on t.Id = ct.TrackId
                JOIN categories c on ct.CategoryId = c.Id
                WHERE ct.CategoryId IN(SELECT Id FROM CategoryHierarchy)
            ";

            return Tracks.FromSqlRaw(rawQuery, id);
        }
    }
}
