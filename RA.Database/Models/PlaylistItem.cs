using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    [Table("PlaylistItems")]
    public class PlaylistItem : BaseModel
    {
        public DateTime ETA { get; set; }

        [Column(TypeName = "double(11,5)")]
        public double Length { get; set; }
        public Track Track { get; set; }

        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }
}
