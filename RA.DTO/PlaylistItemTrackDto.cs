using RA.Database.Models;
using RA.Database.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class PlaylistItemTrackDto
    {
        public int Id { get; set; }
        public string Artists { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }

        public double Duration { get; set; }
        public TrackType Type { get; set; }

        public static PlaylistItemTrackDto FromEntity(Track track)
        {
            return new PlaylistItemTrackDto
            {
                Id = track.Id,
                Artists = track.TrackArtists != null ? string.Join(" / ", track.TrackArtists.Select(ta => ta.Artist.Name)) : String.Empty,
                Title = track.Title,
                FilePath = track.FilePath,
                Duration = track.Duration,
                Type = track.Type,
            };
        }
    }
}
