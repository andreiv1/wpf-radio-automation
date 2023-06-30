using RA.DAL;
using RA.DTO;
using RA.Logic.Tracks.Models;

namespace RA.Logic.Tracks
{
    public class TrackFileImporter : ITrackFilesImporter
    {
        private readonly ITracksService tracksService;

        public TrackFileImporter(ITracksService tracksService)
        {
            this.tracksService = tracksService;
        }

        public void Import(IEnumerable<ProcessingTrack> processingTracks)
        {
            ImportAsync(processingTracks).Wait();
        }

        public async Task ImportSingleAsync(ProcessingTrack processingTrack)
        {
            await ImportAsync(new ProcessingTrack[] { processingTrack });
        }

        public async Task<int> ImportAsync(IEnumerable<ProcessingTrack> processingTracks)
        {
            try
            {
                List<TrackDTO?> toImport = processingTracks
                    .Where(pt => pt.Status == Enums.ProcessingTrackStatus.OK &&
                        pt.TrackDto != null)
                    .Select(pt => pt.TrackDto)
                    .ToList();

                return await tracksService.AddTracks(toImport);
            } catch (Exception )
            {
                return 0;
            }
        }

    }
}
