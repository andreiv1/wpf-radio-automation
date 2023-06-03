using RA.Database.Models.Abstract;
using RA.Database.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    public class ClockItemEvent : ClockItemBase
    {
        public EventType? EventType { get; set; }
        public string EventLabel { get; set; }
        public TimeSpan EstimatedEventStart { get; set; }
        public TimeSpan? EstimatedEventDuration { get; set; }
    }
}
