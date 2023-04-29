using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    [Table("ClockItems")]
    public class ClockItem : BaseModel
    {
        public int OrderIndex { get; set; }

        public int? CategoryId { get; set; }

        public Category? Category { get; set; }

        public int? TrackId { get; set; }
        public Track? Track { get; set; }

        public int? ClockId { get; set; }
        public Clock? Clock { get; set; }

        public int? EventId { get; set; }
        public Event? Event { get; set; }
    }
}
