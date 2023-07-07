using RA.DTO;
using RA.DTO.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public interface IPlaylistsService
    {
        Task AddPlaylistAsync(PlaylistDTO playlistDTO);

        Task<IEnumerable<PlaylistItemDTO>> GetPlaylistItems(int playlistId);
        IEnumerable<PlaylistItemDTO> GetPlaylistItemsByDateTime(DateTime date, int maxHours = 1);
        IEnumerable<PlaylistByHourDTO> GetPlaylistsByHour(DateTime airDate);
        Task<IEnumerable<PlaylistListingDTO>> GetPlaylistsToAirAfterDate(DateTime? date = null);
        Task<bool> PlaylistExists(DateTime date);
    }
}
