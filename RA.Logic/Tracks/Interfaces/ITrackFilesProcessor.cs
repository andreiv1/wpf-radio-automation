using RA.Logic.Tracks.Models;

namespace RA.Logic.Tracks
{
    public interface ITrackFilesProcessor
    {
        Task<int> CountItemsInDirectoryAsync(TrackFilesProcessorOptions options);
        IAsyncEnumerable<ProcessingTrack> ProcessItemsFromDirectoryAsync(TrackFilesProcessorOptions options);
        ProcessingTrack ProcessSingleItem(string path, TrackFilesProcessorOptions options);
        Task<ProcessingTrack> ProcessSingleItemAsync(string path, TrackFilesProcessorOptions options);
    }
}
