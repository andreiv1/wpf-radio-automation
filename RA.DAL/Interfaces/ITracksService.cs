using RA.DTO;

namespace RA.DAL
{
    public interface ITracksService
    {
        IEnumerable<TrackListingDTO> GetTrackList(int skip, int take);
        Task<int> GetTrackCountAsync(string query = "", bool includeDisabled = false);
        Task<TrackDTO> GetTrack(int id);
        Task<IEnumerable<TrackListingDTO>> GetTrackListByArtistAsync(int artistId, int skip, int take);
        IEnumerable<TrackListingDTO> GetTrackListByArtist(int artistId, int skip, int take);
        Task<IEnumerable<TrackListingDTO>> GetTrackListByCategoryAsync(int categoryId, int skip, int take);
        Task<int> AddTracks(IEnumerable<TrackDTO> trackDTOs);
        Task<TrackListingDTO?> GetRandomTrack(int categoryId, List<int>? trackIdsToExclude = null);
        Task<bool> TrackExistsByPath(string filePath);
        Task UpdateTrack(TrackDTO trackDTO);
        Task<bool> DeleteTrack(int trackId);
        Task<IEnumerable<TrackListingDTO>> GetTrackListAsync(int skip, int take, string query = "", bool includeDisabled = false);
        Task<int> GetTrackCountByCategoryAsync(int categoryId);
    }
}
