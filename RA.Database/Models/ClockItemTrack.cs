using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    public class ClockItemTrack : ClockItemBase
    {
        public int? TrackId { get; set; }
        public Track Track { get; set; }
    }
}
