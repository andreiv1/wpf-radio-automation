using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    public class User : BaseModel
    {
        public String Username { get; set; }
        public String Password { get; set; }
        public String FullName { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
    }
}
