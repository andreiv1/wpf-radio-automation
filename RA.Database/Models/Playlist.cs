using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    public class Playlist : BaseModel
    {
        public DateTime AirDate { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateModified { get; set; }
        public ICollection<PlaylistItem> PlaylistItems { get; set; }
    }
}
