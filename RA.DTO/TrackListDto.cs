using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Dto
{
    public class TrackListDto
    {
        public int Id { get; set; }

        public string Artists { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }

        public string FilePath { get; set; }
        public double Duration { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }

        public static TrackListDto FromEntity(Track track)
        {
            return new TrackListDto
            {
                Id = track.Id,
                Artists = track.TrackArtists != null ? string.Join(" / ", track.TrackArtists.Select(ta => ta.Artist.Name)) : String.Empty,
                Title = track.Title,
                Type = track.Type.ToString(),
                FilePath = track.FilePath,
                Duration = track.Duration,
                ReleaseDate = track.ReleaseDate,
                DateAdded = track.DateAdded,
                DateModified = track.DateModified,
                DateDeleted = track.DateDeleted,
            };
        }
    }
}
