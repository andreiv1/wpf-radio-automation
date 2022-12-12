using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    [Table("Track_Tags")]
    public class TrackTag
    {
        public int TrackId { get; set; }
        public Track Track { get; set; }
        public int TagValueId { get; set; }
        public TagValue TagValue { get; set; }
    }
}
