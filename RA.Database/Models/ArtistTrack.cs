using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    [Table("Artists_Tracks")]
    public class ArtistTrack
    {
        public int ArtistId { get; set; }
        public virtual Artist Artist { get; set; }

        public int TrackId { get; set; }
        public virtual Track Track { get; set; }

        public int OrderIndex { get; set; }
    }
}
