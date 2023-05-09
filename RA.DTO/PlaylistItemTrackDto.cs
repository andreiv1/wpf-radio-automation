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
    public class PlaylistItemTrackDTO : PlaylistItemBaseDTO
    {
        public TrackListingDTO Track { get; set; }
        public static PlaylistItem ToEntity(PlaylistItemTrackDTO dto)
        {
            var entity = PlaylistItemBaseDTO.ToEntity(dto);
            entity.Track = new();
            entity.Track.Id = dto.Track.Id;
            return entity;
        }

        public static PlaylistItemTrackDTO FromEntity(PlaylistItem entity)
        {
            return new PlaylistItemTrackDTO
            {
                Id = entity.Id,
                ETA = entity.ETA,
                Length = entity.Length,
                PlaylistId = entity.PlaylistId,
                Track = TrackListingDTO.FromEntity(entity.Track),
            };
        }
    }
}
