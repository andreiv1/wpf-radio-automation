using RA.Database.Models;
using RA.DTO.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class ClockItemTrackDTO : ClockItemBaseDTO
    {
        public int TrackId { get; set; }
        public String Title { get; set; }

        public static ClockItemTrackDTO FromEntity(ClockItemTrack entity)
        {
            return new ClockItemTrackDTO
            {
                Id = entity.Id,
                ClockId = entity.ClockId,
                OrderIndex = entity.OrderIndex,

                TrackId = entity.TrackId,
                Title = entity.Track.Title,
            };
        }

        public static ClockItemTrack ToEntity(ClockItemTrackDTO dto)
        {
            return new ClockItemTrack
            {
                Id = dto.Id,
                ClockId = dto.ClockId,
                OrderIndex = dto.OrderIndex,

                TrackId = dto.TrackId,

            };
        }
    }
}
