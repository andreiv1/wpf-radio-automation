using RA.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL.Interfaces
{
    public interface ITracksService
    {
        Task<IEnumerable<TrackListDto>> GetTrackListAsync(int skip, int take);
        Task<int> GetTrackCountAsync();
        Task<TrackDto> GetTrack(int id);
    }
}
