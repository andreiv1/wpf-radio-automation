using RA.Database;
using RA.DTO;
using RA.DTO.Abstract;
using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.PlanningLogic.Interfaces
{
    public interface IPlaylistGenerator
    {
        Playlist GeneratePlaylist(AppDbContext db,DateTime date);
    }
}
