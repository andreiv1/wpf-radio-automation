using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class PlaylistByHourDTO
    {
        public int PlaylistId { get; set; }
        public int Hour { get; set; }
        public double Length { get; set; } 

    }
}
