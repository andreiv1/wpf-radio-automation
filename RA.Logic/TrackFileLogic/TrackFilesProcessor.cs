using RA.Database.Models.Enums;
using RA.DTO;
using RA.DAL;
using RA.Logic.TrackFileLogic.Enums;
using RA.Logic.TrackFileLogic.Models;

namespace RA.Logic.TrackFileLogic
{
    public class TrackFilesProcessor : ITrackFilesProcessor
    {
        public static readonly string[] SupportedTrackFormats = { ".mp3", ".flac", ".ogg", ".wav" };
        private readonly IArtistsService artistsService;
        private static string defaultTitle = "Unknown Title";
        private static string defaultArtist = "Unknown Artist";

        public TrackFilesProcessor(IArtistsService artistsService)
        {
            this.artistsService = artistsService;
        }

        public async Task<ProcessingTrack> ProcessSingleItemAsync(string path, bool readMetadata = false)
        {
            ITrackMetadataReader metaReader = new TrackMetadataReader(path);
            ProcessingTrack track = new();
            TrackDTO dto = new();
            dto.FilePath = path;
            double duration = (double)(metaReader.GetField(TrackMetadataField.Duration) ?? 0);
            dto.Duration = duration;

            if (readMetadata)
            {
                try
                {
                    dto.Title = metaReader.GetField(TrackMetadataField.Title) as string ?? defaultTitle;
                    var artists = await ProcessArtistsAsync(dto, metaReader.GetField(TrackMetadataField.Artists) as string ?? defaultArtist);
                    dto.Artists = artists.ToList();
                    dto.Album = metaReader.GetField(TrackMetadataField.Album) as string ?? string.Empty;
                    int? year = metaReader.GetField(TrackMetadataField.Year) as int?;
                    if (year.HasValue)
                    {
                        dto.ReleaseDate = new DateTime(year.Value, 1, 1);
                    }
                    dto.Bpm = metaReader.GetField(TrackMetadataField.BPM) as int?;
                    dto.ISRC = metaReader.GetField(TrackMetadataField.ISRC) as String ?? String.Empty;
                    dto.ImageName = metaReader.GetField(TrackMetadataField.Image) as String ?? String.Empty;
                }
                catch (Exception )
                {
                    track.Status = ProcessingTrackStatus.FAILED;
                }
            }
            track.TrackDto = dto;
            return track;
        }
        public IEnumerable<ProcessingTrack> ProcessItemsFromDirectory(TrackFilesProcessorOptions options)
        {
            throw new NotImplementedException();
        }
        private async Task<IEnumerable<TrackArtistDTO>> ProcessArtistsAsync(TrackDTO trackDto, String inputArtists)
        {
            List<TrackArtistDTO> trackArtists = new();
            int orderIndex = 0;
            string[] splitArtists = inputArtists.Split(TrackMetadataReader.defaultArtistDelimiter);

            foreach (var artist in splitArtists)
            {
                var artistTrim = artist.Trim();
                var artistDTO = await artistsService.GetArtistByName(artistTrim);
                if (artistDTO == null)
                {
                    await artistsService.AddArtist(new ArtistDTO { Name = artistTrim });
                    artistDTO = await artistsService.GetArtistByName(artistTrim);
                }
                TrackArtistDTO trackArtistDTO = new TrackArtistDTO
                {
                    ArtistId = artistDTO!.Id,
                    ArtistName = artistDTO?.Name ?? "",
                    OrderIndex = orderIndex,
                };
                trackArtists.Add(trackArtistDTO);
                orderIndex++;
            };
        
            return trackArtists;
        }

        public ProcessingTrack ProcessSingleItem(string path, bool readMetadata = false)
        {
            return ProcessSingleItemAsync(path, readMetadata).Result;
        }
    }
}
