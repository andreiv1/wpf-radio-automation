using RA.Database.Models;
using RA.Database.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class TrackHistoryListingDTO
    {
        public TrackType TrackType { get; set; }
        public DateTime DatePlayed { get; set; }
        public string? Artists { get; set; }
        public string? Title { get; set; }

        public static TrackHistoryListingDTO FromEntity(TrackHistory entity)
        {
            return new TrackHistoryListingDTO
            {
                TrackType = entity.TrackType,
                DatePlayed = entity.DatePlayed,
                Artists = entity.Track?.TrackArtists != null ? string.Join(" / ",
                    entity.Track.TrackArtists.Select(ta => ta.Artist.Name)) : String.Empty,
                Title = entity.Track?.Title,
            };
        }
    }
}
