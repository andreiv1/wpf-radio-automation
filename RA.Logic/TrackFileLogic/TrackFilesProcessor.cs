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
        private readonly ITracksService tracksService;
        private readonly ICategoriesService categoriesService;
        private static string defaultTitle = "Unknown Title";
        private static string defaultArtist = "Unknown Artist";

        public TrackFilesProcessor(IArtistsService artistsService, 
            ITracksService tracksService,
            ICategoriesService categoriesService)
        {
            this.artistsService = artistsService;
            this.tracksService = tracksService;
            this.categoriesService = categoriesService;
        }

        public async Task<ProcessingTrack> ProcessSingleItemAsync(string path, bool readMetadata = false)
        {
            ITrackMetadataReader metaReader = new TrackMetadataReader(path);
            ProcessingTrack track = new();
            TrackDTO dto = new();
            if(await tracksService.TrackExistsByPath(path))
            {
                track.Status = ProcessingTrackStatus.WARNING;
            }
            dto.FilePath = path;
            double duration = (double)(metaReader.GetField(TrackMetadataField.Duration) ?? 0);
            dto.Duration = duration;

            if (readMetadata)
            {
                try
                {
                    dto.Title = metaReader.GetField(TrackMetadataField.Title) as string ?? string.Empty;
                    var artists = await ProcessArtistsAsync(dto, metaReader.GetField(TrackMetadataField.Artists) as string ?? string.Empty);
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
            if (string.IsNullOrEmpty(dto.Title) || dto.Artists?.Count == 0)
            {
                var titleAndArtist = TrackMetadataReader.GetTitleAndArtistFromPath(path);
                if (titleAndArtist.Artist is not null)
                {
                    var artists = titleAndArtist.Artist;
                    var processedArtists = await ProcessArtistsAsync(dto, metaReader.GetField(TrackMetadataField.Artists) as string ?? defaultArtist);
                    dto.Artists = processedArtists.ToList();
                
                }
                dto.Title = titleAndArtist.Title;
            }

            
            track.TrackDto = dto;
            return track;
        }
        public async IAsyncEnumerable<ProcessingTrack> ProcessItemsFromDirectoryAsync(TrackFilesProcessorOptions options)
        {
            if(options.DirectoryPath is not null)
            {
                //Fara traversare in sub-foldere
                var files = Directory.EnumerateFiles(options.DirectoryPath)
                    .Where(f => SupportedTrackFormats.Contains(Path.GetExtension(f).ToLowerInvariant()));
                CategoryDTO categoryDTO = categoriesService.GetCategory(options.MainCategoryId).Result;
                if(categoryDTO != null)
                foreach (var file in files)
                {
                    string fileExtension = Path.GetExtension(file);
                    ProcessingTrack processingTrack = await ProcessSingleItemAsync(file, options.ReadMetadata);
                    if(processingTrack != null)
                    {
                        processingTrack.TrackDto!.Categories = new()
                        {
                            new TrackCategoryDTO()
                            {
                                CategoryId = categoryDTO.Id.GetValueOrDefault(),
                                CategoryName = categoryDTO.Name,
                            }
                        };
                        processingTrack.TrackDto.Type = options.TrackType;
                        processingTrack.TrackDto.Status = options.TrackStatus;
                    }
                    yield return processingTrack!;
                }
            }
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
