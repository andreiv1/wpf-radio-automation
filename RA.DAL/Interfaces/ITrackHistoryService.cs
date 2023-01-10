using RA.Database.Models.Enums;
using RA.DTO;

namespace RA.DAL
{
    public interface ITrackHistoryService
    {
        Task AddTrackToHistory(TrackHistoryDTO trackHistoryDTO);
        Task<ICollection<TrackHistoryListingDTO>> GetHistoryBetween(DateTime dateStart, DateTime dateEnd, IList<TrackType> trackTypes);
        Task<TrackHistoryListingDTO?> RetrieveItem(DateTime datePlayed);
        Task<ICollection<TrackHistoryListingDTO>> RetrieveTrackHistory(DateTime dateMinPlayed);
    }
}