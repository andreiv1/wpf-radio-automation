using RA.Database.Models.Abstract;
using RA.Database.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    public class TrackHistory : BaseModel
    {
        public TrackType TrackType { get; set; }
        public DateTime DatePlayed { get; set; }
        public String Artists { get; set; }
        public String Title { get; set; }
        public TimeSpan Length { get; set; } 
        public String ISRC { get; set; }
        public String Category { get; set; }
        public Track Track { get; set; }
    }
}
