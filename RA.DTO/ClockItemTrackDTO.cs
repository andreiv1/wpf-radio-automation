using RA.Database.Models;
using RA.DTO.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class ClockItemTrackDTO : ClockItemBaseDTO
    {
        public String Title { get; set; }

        public static ClockItemTrackDTO FromEntity(ClockItemTrack entity)
        {
            return new ClockItemTrackDTO
            {

            };
        }
    }
}
