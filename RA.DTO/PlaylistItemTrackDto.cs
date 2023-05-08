using RA.Database.Models;
using RA.Database.Models.Enums;
using RA.DTO.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class PlaylistItemTrackDTO : PlaylistItemBaseDTO
    {
       public int TrackId { get; set; }
    }
}
