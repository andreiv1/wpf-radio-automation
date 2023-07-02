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
        public TimeSpan LengthPlayed { get; set; }
        public Track? Track { get; set; }
        public int? TrackId { get; set; }
    }
}
