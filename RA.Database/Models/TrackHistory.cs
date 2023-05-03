using RA.Database.Models.Abstract;
using RA.Database.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    public class TrackHistory : BaseModel
    {
        public TrackType TrackType { get; set; }
        public DateTime DatePlayed { get; set; }
        [MaxLength(1000)]
        public String Artists { get; set; }
        [MaxLength(200)]
        public String Title { get; set; }
        public TimeSpan LengthPlayed { get; set; }

        [MaxLength(55)]
        public String ISRC { get; set; }

        [MaxLength(100)]
        public String CategoryName { get; set; }
        public Track Track { get; set; }
    }
}
