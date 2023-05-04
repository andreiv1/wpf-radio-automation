using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public interface ITracksService
    {
        IEnumerable<TrackListDTO> GetTrackList(int skip, int take);
        Task<IEnumerable<TrackListDTO>> GetTrackListAsync(int skip, int take);
        Task<int> GetTrackCountAsync();
        Task<TrackDTO> GetTrack(int id);
        Task<IEnumerable<TrackListDTO>> GetTrackListByArtistAsync(int artistId, int skip, int take);
        IEnumerable<TrackListDTO> GetTrackListByArtist(int artistId, int skip, int take);
        Task<IEnumerable<TrackListDTO>> GetTrackListByCategoryAsync(int categoryId, int skip, int take);
        Task AddTracks(IEnumerable<TrackDTO> trackDTOs);
    }
}
