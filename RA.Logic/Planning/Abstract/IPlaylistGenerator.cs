using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.Planning
{
    public interface IPlaylistGenerator
    {
        Task<PlaylistDTO> GeneratePlaylistForDate(DateTime date);
    }
}
