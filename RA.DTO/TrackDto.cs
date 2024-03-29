﻿using RA.Database.Models;
using RA.Database.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class TrackDTO
    {
        public int Id { get; set; }
        public TrackType Type { get; set; }
        public TrackStatus Status { get; set; }
        public string? Title { get; set; }
        public double Duration { get; set; }
        public double? StartCue { get; set; }
        public double? NextCue { get; set; }
        public double? EndCue { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? Album { get; set; }
        public string? Comments { get; set; }
        public string? FilePath { get; set; }
        public string? ImageName { get; set; }
        public int? Bpm { get; set; }
        public string? ISRC { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
        public List<TrackArtistDTO> Artists { get; set; }
        public List<TrackCategoryDTO> Categories { get; set; }
        public List<TrackTagDTO> Tags { get; set; }
        public static TrackDTO FromEntity(Track track)
        {
            var dto = new TrackDTO
            {
                Id = track.Id,
                Type = track.Type,
                Status = track.Status,
                Title = track.Title,
                Duration = track.Duration,
                StartCue = track.StartCue,
                NextCue = track.NextCue,
                EndCue = track.EndCue,
                ReleaseDate = track.ReleaseDate,
                Album = track.Album,
                Comments = track.Comments,
                FilePath = track.FilePath,
                ImageName = track.ImageName,
                Bpm = track.Bpm,
                ISRC = track.ISRC,
                DateAdded = track.DateAdded,
                DateModified = track.DateModified,
                DateDeleted = track.DateDeleted,
                Artists = track.TrackArtists != null ?
                    track.TrackArtists.Select(ta => TrackArtistDTO.FromEntity(ta)).ToList() : new(),
                Categories = track.Categories != null ?
                    track.Categories.Select(c => TrackCategoryDTO.FromEntity(c)).ToList() : new(),
                Tags = track.TrackTags != null ?
                    track.TrackTags.Select(tt => TrackTagDTO.FromEntity(tt)).ToList() : new(),
            };
            return dto;
        }

        public static Track ToEntity(TrackDTO dto)
        {
            return new Track
            {
                Id = dto.Id,
                TrackArtists = dto.Artists != null ? dto.Artists.Select(a => TrackArtistDTO.ToEntity(a)).ToList() : null,
                Categories = dto.Categories != null ? dto.Categories.Select(c => TrackCategoryDTO.ToEntity(c)).ToList() : null,
                TrackTags = dto.Tags != null ? dto.Tags.Select(t => TrackTagDTO.ToEntity(t)).ToList() : null,
                StartCue = dto.StartCue,
                EndCue = dto.EndCue,
                NextCue = dto.NextCue,
                Type = dto.Type,
                Status = dto.Status,
                Title = dto.Title,
                Duration = dto.Duration,
                ReleaseDate = dto.ReleaseDate,
                Album = dto.Album,
                Comments = dto.Comments,
                FilePath = dto.FilePath,
                ImageName = dto.ImageName,
                Bpm = dto.Bpm,
                ISRC = dto.ISRC,
            };
        }
    }
}
