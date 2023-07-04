using RA.DTO;
using RA.Logic.Tracks.Enums;

namespace RA.Logic.Tracks.Models
{
    public class ProcessingTrack
    {
        public TrackDTO? TrackDto { get; set; }
        public ProcessingTrackStatus Status { get; set; }

        public String OriginalPath { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public string? Categories => TrackDto?.Categories?.Count > 0 ?
            string.Join("; ", TrackDto.Categories.Select(c => c.CategoryName)) : null;

        public string? Artists => TrackDto?.Artists?.Count > 0 ?
            string.Join(" / ", TrackDto.Artists.Select(a => a.ArtistName)) : null;
    }
}
