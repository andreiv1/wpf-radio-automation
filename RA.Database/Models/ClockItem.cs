using RA.Database.Models.Abstract;
using RA.Database.Models.Enums;
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

        #region Category
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public int? ArtistSeparation { get; set; }
        public int? TitleSeparation { get; set; }

        public int? TrackSeparation { get; set; }

        #endregion
        public int? TrackId { get; set; }
        public Track Track { get; set; }

        #region Event
        public EventType? EventType { get; set; }
        public string EventLabel { get; set; }
        public TimeSpan? EstimatedEventDuration { get; set; }

        #endregion
        public int? ClockId { get; set; }
        public Clock Clock { get; set; }

    }
}
