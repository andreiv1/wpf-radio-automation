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

        public void Import(IEnumerable<ProcessingTrack> processingTracks, TrackFilesProcessorOptions options)
        {
            ImportAsync(processingTracks,options).Wait();
        }

        public async Task ImportSingleAsync(ProcessingTrack processingTrack, TrackFilesProcessorOptions options)
        {
            await ImportAsync(new ProcessingTrack[] { processingTrack },options);
        }

        public async Task<int> ImportAsync(IEnumerable<ProcessingTrack> processingTracks,
                                           TrackFilesProcessorOptions options)
        {
            try
            {
                List<TrackDTO> tracksToImport = new();
                foreach (var processingTrack in processingTracks)
                {
                    if (processingTrack.Status == Enums.ProcessingTrackStatus.OK &&
                        processingTrack.TrackDto != null)
                    {
                        tracksToImport.Add(processingTrack.TrackDto);
                        if(options.NewDirectoryOption != NewDirectoryOption.LeaveCurrent && options.NewDirectoryPath != null)
                        {
                            var newFilePath = Path.Combine(options.NewDirectoryPath,
                                Path.GetFileName(processingTrack.OriginalPath));

                            switch(options.NewDirectoryOption)
                            {
                                case NewDirectoryOption.CopyToNewLocation:
                                    await CopyTrack(processingTrack.OriginalPath, newFilePath); 
                                    break;
                                case NewDirectoryOption.MoveToNewLocation:
                                    await MoveTrack(processingTrack.OriginalPath, newFilePath);
                                    break;
                            }

                            processingTrack.TrackDto.FilePath = newFilePath;

                        }
                    }
                }

                return await tracksService.AddTracks(tracksToImport);
            } catch (Exception )
            {
                return 0;
            }
        }

        private async Task MoveTrack(string originalPath, string movePath)
        {
      
                await Task.Run(() => {
                    try
                    {
                        File.Move(originalPath, movePath);
                        // File moved successfully
                    }
                    catch (IOException e)
                    {
                        // Handle any exceptions occurred during the move operation
                        Console.WriteLine($"An error occurred while moving the file: {e.Message}");
                    }
                });
        }

        private async Task CopyTrack(string originalPath, string copyPath)
        {
            await Task.Run(() =>
            {
                try
                {
                    File.Copy(originalPath, copyPath);
                    // File moved successfully
                }
                catch (IOException e)
                {
                    // Handle any exceptions occurred during the move operation
                    Console.WriteLine($"An error occurred while moving the file: {e.Message}");
                }
            });
        }
    }
}
