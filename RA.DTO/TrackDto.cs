using RA.Database.Models;
using RA.Database.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class TrackDto
    {
        public int Id { get; set; }
        public TrackType Type { get; set; }
        public TrackStatus Status { get; set; }
        public string? Title { get; set; }
        public double Duration { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? Album { get; set; }
        public string? Comments { get; set; }
        public string? Lyrics { get; set; }
        public string? FilePath { get; set; }
        public string? ImageName { get; set; }
        public int? Bpm { get; set; }
        public string ISRC { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
        public List<TrackArtistDto>? Artists { get; set; }

        public List<TrackCategoryDto>? Categories { get; set; }
        public static TrackDto FromEntity(Track track)
        {
            return new TrackDto
            {
                Id = track.Id,
                Type = track.Type,
                Status = track.Status,
                Title = track.Title,
                Duration = track.Duration,
                ReleaseDate = track.ReleaseDate,
                Album = track.Album,
                Comments = track.Comments,
                Lyrics = track.Lyrics,
                FilePath = track.FilePath,
                ImageName = track.ImageName,
                Bpm = track.Bpm,
                ISRC = track.ISRC,
                DateAdded = track.DateAdded,
                DateModified = track.DateModified,
                DateDeleted = track.DateDeleted,
                Artists = track.TrackArtists != null ?
                    track.TrackArtists.Select(ta => TrackArtistDto.FromEntity(ta)).ToList() : null,
                Categories = track.Categories != null ?
                    track.Categories.Select(c => TrackCategoryDto.FromEntity(c)).ToList() : null,
            };
        }

        public static Track ToEntity(TrackDto dto)
        {
            return new Track
            {
                Id = dto.Id,
                TrackArtists = dto.Artists != null ? dto.Artists.Select(a => TrackArtistDto.ToEntity(a)).ToList() : null,
                Categories = dto.Categories != null ? dto.Categories.Select(c => TrackCategoryDto.ToEntity(c)).ToList() : null,
                Type = dto.Type,
                Status = dto.Status,
                Title = dto.Title,
                Duration = dto.Duration,
                ReleaseDate = dto.ReleaseDate,
                Album = dto.Album,
                Comments = dto.Comments,
                Lyrics = dto.Lyrics,
                FilePath = dto.FilePath,
                ImageName = dto.ImageName,
                Bpm = dto.Bpm,
                ISRC = dto.ISRC,
                DateAdded = DateTime.Now,
            };
        }
    }
}
