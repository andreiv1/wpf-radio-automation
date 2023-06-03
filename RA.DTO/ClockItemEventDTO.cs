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
    public class ClockItemEventDTO : ClockItemBaseDTO
    {
        public EventType? EventType { get; set; }
        public string EventLabel { get; set; }

        public TimeSpan EstimatedEventStart { get; set; }
        public TimeSpan? EstimatedEventDuration { get; set; }

        public static ClockItemEventDTO FromEntity(ClockItemEvent entity)
        {
            return new ClockItemEventDTO
            {
                Id = entity.Id,
                ClockId = entity.ClockId,
                OrderIndex = entity.OrderIndex,

                EstimatedEventStart = entity.EstimatedEventStart,
                EstimatedEventDuration = entity.EstimatedEventDuration,
                EventLabel = entity.EventLabel,
                EventType = entity.EventType,
            };
        }

        public static ClockItemEvent ToEntity(ClockItemEventDTO dto)
        {
            return new ClockItemEvent
            {
                Id = dto.Id,
                ClockId = dto.ClockId,
                OrderIndex = dto.OrderIndex,

                EstimatedEventStart = dto.EstimatedEventStart,
                EstimatedEventDuration = dto.EstimatedEventDuration,
                EventLabel = dto.EventLabel,
                EventType = dto.EventType,
            };
        }
    }
}
