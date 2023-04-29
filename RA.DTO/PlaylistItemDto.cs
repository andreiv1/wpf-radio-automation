using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Dto
{
    public class PlaylistItemDto
    {
        public int Id { get; set; }
        public DateTime ETA { get; set; }
        public double Length { get; set; }
        public int PlaylistId { get; set; }

        public PlaylistItemTrackDto TrackDto { get; set; }

        public static PlaylistItemDto FromEntity(PlaylistItem entity)
        {
            return new PlaylistItemDto
            {
                Id = entity.Id,
                ETA = entity.ETA,
                Length = entity.Length,
                PlaylistId = entity.PlaylistId,
                TrackDto = entity.Track is not null ? PlaylistItemTrackDto.FromEntity(entity.Track) 
                    : throw new Exception("Track is required."),
            };
        }
    }
}
