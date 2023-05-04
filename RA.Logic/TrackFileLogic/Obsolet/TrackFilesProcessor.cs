using RA.Database;
using RA.DTO;
using RA.Logic.TrackFileLogic.Enums;
using RA.Logic.TrackFileLogic.Exceptions;
using RA.Database.Models;
using RA.Database.Models.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib.Matroska;
using TrackType = RA.Database.Models.Enums.TrackType;
using RA.Logic.TrackFileLogic.Models;

namespace RA.Logic.TrackFileLogic
{
    [Obsolete]
    public class OldTrackFilesProcessor
    {
        public static readonly string[] SupportedTrackFormats = { ".mp3", ".flac", ".ogg", ".wav" };

        /// <summary>
        /// Event to trigger after processing a single item when processing multiple items in bulk
        /// </summary>
        public int TotalItems { get; private set; } = 0;
        public int ValidItems { get; private set; } = 0;
        public int InvalidItems { get; private set; } = 0;
        public int WarningItems { get; private set; } = 0;
        //TODO: Process Items from subfolders etc.

        /// <summary>
        /// Processes all supported track files within a specified directory and associates them with the given category, track type, and track status.
        /// The method enumerates the files in the directory, filters them based on supported track formats, and processes each file individually.
        /// It also calculates and maintains the count of valid, invalid, and warning items during processing.
        /// </summary>
        /// <param name="directoryPath">The path of the directory containing the track files to be processed.</param>
        /// <param name="categoryId">The ID of the category to associate with the processed tracks.</param>
        /// <param name="trackType">The type of the track to be associated with the processed tracks (default: TrackType.Other).</param>
        /// <param name="trackStatus">The status of the track to be associated with the processed tracks (default: TrackStatus.Enabled).</param>
        /// <param name="readMetadata">The metadata will read if set to True. By default, it is false.</param>
        /// <returns>A list of processed tracks with information about their processing status and associated metadata.</returns>
        /// <exception cref="CategoryNotFoundException">Thrown when the specified category ID does not exist in the database.</exception>
        public List<ProcessingTrack> ProcessItemsFromDirectory(string directoryPath, int categoryId,
            TrackType trackType = TrackType.Other, TrackStatus trackStatus = TrackStatus.Enabled, bool readMetadata = false)
        {
            //String? categoryName = null;
            //using (var db = new AppDbContext())
            //{
            //    categoryName = db.Categories
            //        .Where(c => c.Id == categoryId)
            //        .Select(c => c.Name)
            //        .FirstOrDefault();
            //}
            //if (categoryName is null)
            //{
            //    throw new CategoryNotFoundException($"Category with id {categoryId} does not exist!");
            //}
            //TrackCategoryDto trackCategoryDto = new TrackCategoryDto
            //{
            //    CategoryId = categoryId,
            //    CategoryName = categoryName ?? String.Empty
            //};
            //List<ProcessingTrack> processingTracks = new();
            //if (directoryPath is not null)
            //{
            //    var files = Directory.EnumerateFiles(directoryPath).Where(f => SupportedTrackFormats.Contains(Path.GetExtension(f).ToLowerInvariant()));
            //    TotalItems = files.Count();
            //    foreach (var file in files)
            //    {
            //        string fileExtension = Path.GetExtension(file);

            //        //TODO: Handle exception(s) - format problem
            //        var processingTrack = ProcessSingleItem(file, readMetadata);
            //        if (processingTrack.TrackDto is not null)
            //        {
            //            //processingTrack.TrackDto.Type = trackType;
            //            processingTrack.TrackDto.Status = trackStatus;
            //            processingTrack.TrackDto.Categories = new()
            //        {
            //            trackCategoryDto
            //        };
            //            switch (processingTrack.Status)
            //            {
            //                case ProcessingTrackStatus.OK:
            //                    ValidItems++;
            //                    break;
            //                case ProcessingTrackStatus.FAILED:
            //                    InvalidItems++;
            //                    break;
            //                case ProcessingTrackStatus.WARNING:
            //                    WarningItems++;
            //                    break; ;
            //            }
            //            processingTracks.Add(processingTrack);
            //            TrackProcessed?.Invoke(this, new ProcessingTrackEventArgs(processingTrack));
            //        }
            //    }
            //}

            throw new NotImplementedException();
        }

        //TODO: Check processingTrack for warnings
        private bool CheckTrackForWarnings()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Processes the artists for a given track, splitting the input artists string by the default artist delimiter.
        /// The method checks for the existence of each artist in the database, and if the artist is not found, a new record is created.
        /// It then creates a list of TrackArtistDto objects with the processed artist information and the order index.
        /// </summary>
        /// <param name="trackDto">The track DTO associated with the artists being processed.</param>
        /// <param name="inputArtists">The input artists string to be processed and split.</param>
        /// <returns>A list of TrackArtistDto objects representing the processed artists and their order index.</returns>
        private List<TrackArtistDTO> ProcessArtists(TrackDTO trackDto, String inputArtists)
        {
            //List<TrackArtistDto> trackArtists = new();
            ////TODO delimiter might not be well placed here
            //string[] splitArtists = inputArtists.Split(TrackMetadataReader.defaultArtistDelimiter);
            //int orderIndex = 0;
            //using (var db = new AppDbContext())
            //{
            //    foreach (var artistName in splitArtists)
            //    {
            //        var query = db.Artists.Where(a => a.Name == artistName);
            //        ArtistDto artistDto;
            //        if (query.Any())
            //        {
            //            artistDto = query.Select(a => ArtistDTO.FromEntity(a)).First();
            //        }
            //        else
            //        {
            //            Artist newArtist = new Artist()
            //            {
            //                Name = artistName,
            //            };

            //            db.Artists.Add(newArtist);
            //            db.SaveChanges();

            //            artistDto = ArtistDTO.FromEntity(newArtist);
            //        }
            //        TrackArtistDto trackArtistDto = new()
            //        {
            //            ArtistId = artistDto.Id,
            //            ArtistName = artistName,
            //            OrderIndex = orderIndex,
            //        };
            //        trackArtists.Add(trackArtistDto);
            //        orderIndex++;
            //    }
            //}

            //return trackArtists;
            throw new NotImplementedException();
        }

        private static string defaultTitle = "Unknown Title";
        private static string defaultArtist = "Unknown Artist";

        /// <summary>
        /// Processes a single track file at the specified path and extracts its metadata using TrackMetadataReader.
        /// The method creates a new TrackDto object, fills it with the extracted metadata, and assigns it to a new ProcessingTrack object.
        /// If an exception occurs during the processing, the ProcessingTrack object's status is set to FAILED.
        /// </summary>
        /// <param name="path">The path of the track file to be processed.</param>
        /// <param name="readMetadata">The metadata will read if set to True. By default, it is false.</param>
        /// <returns>A ProcessingTrack object with the processed track's metadata and its processing status.</returns>
        public ProcessingTrack ProcessSingleItem(string path, bool readMetadata = false)
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
                    dto.Artists = ProcessArtists(dto,
                        metaReader.GetField(TrackMetadataField.Artists) as string ?? defaultArtist);
                    dto.Album = metaReader.GetField(TrackMetadataField.Album) as string ?? string.Empty;
                    int? year = metaReader.GetField(TrackMetadataField.Year) as int?;
                    if (year.HasValue)
                    {
                        dto.ReleaseDate = new DateTime(year.Value, 1, 1);
                    }
                    int? bpm = metaReader.GetField(TrackMetadataField.BPM) as int?;
                    if (bpm.HasValue)
                    {
                        dto.Bpm = bpm.Value;
                    }
                    dto.ImageName = metaReader.GetField(TrackMetadataField.Image) as string ?? string.Empty;
                    String ISRC = metaReader.GetField(TrackMetadataField.ISRC) as String ?? String.Empty;
                    dto.ISRC = ISRC;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    track.Status = ProcessingTrackStatus.FAILED;
                }
                
            } 
            if(!readMetadata || ((dto.Title ?? String.Empty).Equals(defaultTitle) && dto.Artists.Count == 0))
            {
                var titleAndArtist = TrackMetadataReader.GetTitleAndArtistFromPath(path);
                if (titleAndArtist.Artist is not null)
                {
                    var artists = titleAndArtist.Artist;
                    dto.Artists = ProcessArtists(dto, artists);
                }
                dto.Title = titleAndArtist.Title;
            }

            track.Status = ProcessingTrackStatus.OK;
            track.TrackDto = dto;

            return track;
        }
    }
}
