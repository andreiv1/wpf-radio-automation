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
        IEnumerable<PlaylistItemBaseDTO> GetPlaylistItems(int playlistId);
        IEnumerable<PlaylistItemBaseDTO> GetPlaylistItemsByDateTime(DateTime date, int maxHours = 1);
        IEnumerable<PlaylistByHourDTO> GetPlaylistsByHour(DateTime airDate);
        IEnumerable<PlaylistListingDTO> GetPlaylistsToAirAfterDate(DateTime? date = null);
    }
}
