using RA.DTO;

namespace RA.DAL
{
    public interface ITrackHistoryService
    {
        Task AddTrackToHistory(TrackHistoryDTO trackHistoryDTO);
        Task<TrackHistoryListingDTO?> RetrieveItem(DateTime datePlayed);
        Task<ICollection<TrackHistoryListingDTO>> RetrieveTrackHistory(DateTime dateMinPlayed);
    }
}