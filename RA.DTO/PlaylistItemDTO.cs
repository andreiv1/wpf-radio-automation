using RA.Database.Models;
using RA.Database.Models.Enums;
using RA.DTO.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class PlaylistItemDTO 
    {
        public int Id { get; set; }
        public DateTime ETA { get; set; }
        public DateTime? NiceETA { get; set; }
        public double Length { get; set; }
        public int PlaylistId { get; set; }
        public EventType? EventType { get; set; }
        public TrackListingDTO? Track { get; set; }
        public string? Label { get; set; } 
        public int? ParentId { get; set; }
        public static PlaylistItem ToEntity(PlaylistItemDTO dto)
        {
            var entity = new PlaylistItem()
            {
                Id = dto.Id,
                ETA = dto.ETA,
                Length = dto.Length,
                PlaylistId = dto.PlaylistId,
                Track = dto.Track != null ? new Track { Id = dto.Track.Id} : null,
                ParentPlaylistItem = dto.ParentId.HasValue ? new PlaylistItem { Id = dto.ParentId.Value } : null,
                Label = dto.Label,
                ParentPlaylistItemId = dto.ParentId,
                EventType = dto.EventType,
            };
            return entity;
        }

        public static PlaylistItemDTO FromEntity(PlaylistItem entity)
        {
            return new PlaylistItemDTO
            {
                Id = entity.Id,
                ETA = entity.ETA,
                Length = entity.Length,
                PlaylistId = entity.PlaylistId,
                Track = entity.Track != null ? TrackListingDTO.FromEntity(entity.Track) : null,
                Label = entity.Label,
                ParentId = entity.ParentPlaylistItemId,
                NiceETA = entity.NiceETA,
                EventType = entity.EventType,
            };
        }
    }
}
