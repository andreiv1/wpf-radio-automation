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
    public class ClockItemTrackDTO : ClockItemBaseDTO
    {
        public int TrackId { get; set; }

        public String? Artists { get; set; }
        public String? Title { get; set; }
        public TrackType TrackType { get; set; }
        public TimeSpan TrackDuration { get; set; }

        public bool IsFiller => false;

        public static ClockItemTrackDTO FromEntity(ClockItemTrack entity)
        {
            return new ClockItemTrackDTO
            {
                Id = entity.Id,
                ClockId = entity.ClockId,
                OrderIndex = entity.OrderIndex,

                TrackId = entity.TrackId,
                Artists = entity.Track?.TrackArtists != null ? string.Join(" / ", 
                    entity.Track.TrackArtists.Select(ta => ta.Artist.Name)) : null,
                Title = entity.Track?.Title ?? "",
                TrackType = entity.Track?.Type ?? TrackType.Other,
                TrackDuration = TimeSpan.FromSeconds(entity.Track?.Duration ?? 0),

                ClockItemEventId = entity.ClockItemEventId,
                EventOrderIndex = entity.EventOrderIndex,
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
