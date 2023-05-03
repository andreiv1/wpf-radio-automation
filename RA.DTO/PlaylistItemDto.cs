using RA.Database.Models;
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
        public double Length { get; set; }
        public int PlaylistId { get; set; }

        public PlaylistItemTrackDTO TrackDto { get; set; }

        public static PlaylistItemDTO FromEntity(PlaylistItem entity)
        {
            return new PlaylistItemDTO
            {
                Id = entity.Id,
                ETA = entity.ETA,
                Length = entity.Length,
                PlaylistId = entity.PlaylistId,
                TrackDto = entity.Track is not null ? PlaylistItemTrackDTO.FromEntity(entity.Track) 
                    : throw new Exception("Track is required."),
            };
        }
    }
}
