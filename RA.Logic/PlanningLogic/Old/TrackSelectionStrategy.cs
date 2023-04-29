using Microsoft.EntityFrameworkCore;
using RA.Database;
using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.PlanningLogic.Old
{
    public interface ITrackSelectionStrategy
    {
        Track? SelectTrack(int categoryId, ICollection<PlaylistItem> playlistItems);
    }
    public class TrackDefaultSelectionStrategy : ITrackSelectionStrategy
    {
        private readonly AppDbContext db;
        private readonly Random random;
        public TrackDefaultSelectionStrategy(AppDbContext db)
        {
            this.db = db;
            random = new Random();
        }

        //Random track selection with default 30 min track separation.

        const int defaultMinSeparation = 30;

        public bool PlaylistTrackValidation(int trackId, ICollection<PlaylistItem> playlistItems, int trackSeparation)
        {
            if (playlistItems.Count == 0) return true;

            var lastItem = playlistItems.Last();
            DateTime timeThreshold = lastItem.ETA.AddMinutes(-trackSeparation);

            return !playlistItems.Any(pi => pi.Track.Id == trackId &&
                pi.ETA > timeThreshold);
        }
        public Track SelectTrack(int categoryId, ICollection<PlaylistItem> playlistItems)
        {
            var query = db.Tracks;
            int count = query.Count();
            int randomIndex = random.Next(0, count);

            Track randomTrack = query.ElementAt(randomIndex);
            return randomTrack;
            //Track? randomTrack = null;
            //var categoryQuery = db.Categories
            //    .Include(c => c.Tracks)
            //    .Where(c => c.Id == categoryId);

            //var categoryTracks = categoryQuery.First().Tracks;

            //int count = categoryTracks.Count();
            //int randomIndex = random.Next(0, count);

            //if(categoryTracks.Count > 0)
            //    randomTrack = categoryTracks.ElementAt(randomIndex);

            //return randomTrack;
        }

    }
}
