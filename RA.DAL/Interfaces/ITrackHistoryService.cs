using RA.DTO;

namespace RA.DAL
{
    public interface ITrackHistoryService
    {
        Task AddTrackToHistory(TrackHistoryDTO trackHistoryDTO);
    }
}