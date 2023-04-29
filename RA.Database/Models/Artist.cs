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
    [Table("Artists")]
    public class Artist : BaseModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<ArtistTrack> ArtistTracks { get; set; }

    }
}
