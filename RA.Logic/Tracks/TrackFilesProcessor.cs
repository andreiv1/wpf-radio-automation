using RA.Database.Models.Enums;
using RA.DTO;
using RA.DAL;
using RA.Logic.Tracks.Enums;
using RA.Logic.Tracks.Models;

namespace RA.Logic.Tracks
{
    public class TrackFilesProcessor : ITrackFilesProcessor
    {
        public static readonly string[] SupportedTrackFormats = { ".mp3", ".flac", ".ogg", ".wav" };
        private readonly IArtistsService artistsService;
        private readonly ITracksService tracksService;
        private readonly ICategoriesService categoriesService;
        private readonly ITagsService tagsService;
        private static readonly string defaultTitle = "Unknown Title";
        private static readonly string defaultArtist = "Unknown Artist";

        public TrackFilesProcessor(IArtistsService artistsService,
                                   ITracksService tracksService,
                                   ICategoriesService categoriesService,
                                   ITagsService tagsService)
        {
            this.artistsService = artistsService;
            this.tracksService = tracksService;
            this.categoriesService = categoriesService;
            this.tagsService = tagsService;
        }

        public async Task<ProcessingTrack> ProcessSingleItemAsync(string path, TrackFilesProcessorOptions options)
        {
            
            ProcessingTrack track = new();
            TrackDTO dto = new();

            track.OriginalPath = path;
            if(options.NewDirectoryOption != NewDirectoryOption.LeaveCurrent && options.NewDirectoryPath != null)
            {
                path = Path.Combine(options.NewDirectoryPath,
                                Path.GetFileName(path));
            }
            if (await tracksService.TrackExistsByPath(path))
            {
                track.Status = ProcessingTrackStatus.WARNING;
                track.Message = $"Found duplicate at path: {path}";
            }

            dto.FilePath = path;
            ITrackMetadataReader? metaReader = null;
            try
            {
                metaReader = new TrackMetadataReader(track.OriginalPath);
                double duration = (double)(metaReader.GetField(TrackMetadataField.Duration) ?? 0);
                dto.Duration = duration;
            }
            catch(Exception e)
            {
                track.Status = ProcessingTrackStatus.FAILED;
                track.Message = $"Error: {e.Message}";
            }

            if (metaReader != null && options.ReadMetadata)
            {
                try
                {
                    var artists = await ProcessArtistsAsync(metaReader.GetField(TrackMetadataField.Artists) as string ?? string.Empty);
                    var genres = await ProcessGenresAsync(metaReader.GetField(TrackMetadataField.Genres) as string ?? string.Empty);
                    dto.Title = metaReader.GetField(TrackMetadataField.Title) as string ?? string.Empty;
                    
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
                    dto.Tags = genres.ToList();
                    
                }
                catch (Exception e)
                {
                    track.Status = ProcessingTrackStatus.FAILED;
                    track.Message = $"Error: {e.Message}";
                }
            }
            if (string.IsNullOrEmpty(dto.Title) || dto.Artists?.Count == 0)
            {
                var titleAndArtist = TrackMetadataReader.GetTitleAndArtistFromPath(path);
                if (titleAndArtist.Artist != null)
                {
                    var processedArtists = await ProcessArtistsAsync(metaReader?.GetField(TrackMetadataField.Artists) as string ?? defaultArtist);
                    dto.Artists = processedArtists.ToList();

                }
                dto.Title = titleAndArtist.Title;
            }
            track.TrackDto = dto;
            return track;
        }
        public async IAsyncEnumerable<ProcessingTrack> ProcessItemsFromDirectoryAsync(TrackFilesProcessorOptions options)
        {
            if (options.DirectoryPath is not null)
            {
                if (options.ScanSubfolders && options.SubfolderScanOption == SubfolderScanOption.CreateNewChildrenCategoryForEachExistingCategory)
                {
                    //throw new NotImplementedException();
                }
                else
                {
                    IEnumerable<string> files;

                    if (options.ScanSubfolders && options.SubfolderScanOption == SubfolderScanOption.PutAllInSameCategory)
                    {
                        files = Directory.EnumerateFiles(options.DirectoryPath, "*.*", SearchOption.AllDirectories)
                            .Where(f => SupportedTrackFormats.Contains(Path.GetExtension(f).ToLowerInvariant()));
                    }
                    else
                    {
                        files = Directory.EnumerateFiles(options.DirectoryPath, "*.*", SearchOption.TopDirectoryOnly)
                            .Where(f => SupportedTrackFormats.Contains(Path.GetExtension(f).ToLowerInvariant()));
                    }

                    CategoryDTO categoryDTO = categoriesService.GetCategory(options.MainCategoryId).Result;
                    if (categoryDTO != null)
                        foreach (var file in files)
                        {
                            string fileExtension = Path.GetExtension(file);
                            ProcessingTrack processingTrack = await ProcessSingleItemAsync(file, options);
                            if (processingTrack != null)
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
        }

        public async Task<int> CountItemsInDirectoryAsync(TrackFilesProcessorOptions options)
        {
            int count = await Task.Run(() =>
            {
                int filesCount = 0;
                IEnumerable<string> files;
                if (options.DirectoryPath != null)
                {
                    if (options.ScanSubfolders && options.SubfolderScanOption == SubfolderScanOption.PutAllInSameCategory)
                    {
                        files = Directory.EnumerateFiles(options.DirectoryPath, "*.*", SearchOption.AllDirectories)
                            .Where(f => SupportedTrackFormats.Contains(Path.GetExtension(f).ToLowerInvariant()));
                    }
                    else
                    {
                        files = Directory.EnumerateFiles(options.DirectoryPath, "*.*", SearchOption.TopDirectoryOnly)
                            .Where(f => SupportedTrackFormats.Contains(Path.GetExtension(f).ToLowerInvariant()));
                    }
                    if (options.DirectoryPath is not null)
                    {
                        try
                        {
                            filesCount = files.Count();
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            Console.WriteLine("Access to directory denied: " + ex.Message);
                        }
                        catch (DirectoryNotFoundException ex)
                        {
                            Console.WriteLine("Directory not found: " + ex.Message);
                        }
                    }
                }
                return filesCount;
                    
            });

            return count;
        }
        private async Task<IEnumerable<TrackArtistDTO>> ProcessArtistsAsync(string inputArtists)
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

        private async Task<IEnumerable<TrackTagDTO>> ProcessGenresAsync(string inputGenres)
        {
            List<TrackTagDTO> genresValues = new();
            string[] splitGenres = inputGenres.Split(TrackMetadataReader.splitTokens, StringSplitOptions.RemoveEmptyEntries);
            foreach (var genre in splitGenres)
            {
                var tagValue = await tagsService.AddTagValue("Genre", genre.Trim());
                if (tagValue != null)
                {
                    genresValues.Add(new TrackTagDTO
                    {
                        TagCategoryId = 1, //BuiltIn
                        TagValueId = tagValue.Id,
                    });
                }
                else
                {
                    DebugHelper.WriteLine(this, $"Tag value {genre} couldn't be inserted");
                }
                }
                return genresValues;
        }

        public ProcessingTrack ProcessSingleItem(string path, TrackFilesProcessorOptions options)
        {
            return ProcessSingleItemAsync(path, options).Result;
        }
    }
}
