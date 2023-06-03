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
        public TimeSpan? EstimatedEventDuration { get; set; }

        public static ClockItemEventDTO FromEntity(ClockItemEvent entity)
        {
            return new ClockItemEventDTO
            {

            };
        }
    }
}
