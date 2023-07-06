using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class TrackListingDTO
    {
        public int Id { get; set; }
        public string? Artists { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
        public string? FilePath { get; set; }
        public string? ImageName { get; set; }
        public double Duration { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
        public string? Status { get; set; }
        public string? Categories { get; set; }
        public static TrackListingDTO FromEntity(Track track)
        {
            return new TrackListingDTO
            {
                Id = track.Id,
                Artists = track.TrackArtists != null ? string.Join(" / ", track.TrackArtists.Select(ta => ta.Artist.Name)) : String.Empty,
                Title = track.Title,
                Type = track.Type.ToString(),
                FilePath = track.FilePath,
                ImageName = track.ImageName,
                Duration = track.Duration,
                ReleaseDate = track.ReleaseDate,
                DateAdded = track.DateAdded,
                DateModified = track.DateModified,
                DateDeleted = track.DateDeleted,
                Status = track.Status.ToString(),
                Categories = track.Categories != null ? string.Join(", ", track.Categories.Select(c => c.Name)) : String.Empty,
            };
        }
    }
}
