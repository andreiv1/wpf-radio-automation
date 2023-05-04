using RA.Logic.TrackFileLogic.Enums;
using RA.Logic.TrackFileLogic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.TrackFileLogic
{
    public class TrackMetadataReader : ITrackMetadataReader
    {
        #region Fields
        /// <summary>
        /// Split tokens for multiple artists inside a field
        /// </summary>
        public static readonly string[] splitTokens = new string[] { ";", "; ", " ; ", "/", " / ", "/ " };

        public static readonly string defaultArtistDelimiter = " / ";
        private readonly string filePath;
        private Dictionary<TrackMetadataField, object> metadataDictionary;
        #endregion

        public static String? ImagePath { get; set; }

        #region Constructor
        public TrackMetadataReader(String filePath)
        {
            this.filePath = filePath;
            this.metadataDictionary = new();
            ReadMetadata();
        }
        #endregion Constructor
        public object? GetField(TrackMetadataField field)
        {
            object? f;
            metadataDictionary.TryGetValue(field, out f);
            return f;
        }

        /// <summary>
        /// Reads the metadata of an audio track using the TagLib library, and extracts various metadata fields.
        /// <exception cref="TrackUnsupportedFormatException">Thrown when the file's format is not supported by the TagLib library</exception>
        /// <exception cref="TrackFileReadException">Thrown when file coudn't be read (IOException)</exception>
        /// <exception cref="TrackCorruptFileException">Thrown when the file is corrupt or has an unsupported format, as indicated by a TagLib.CorruptFileException.</exception>
        private void ReadMetadata()
        {
            try
            {
                using var tagLibFile = TagLib.File.Create(filePath);

                string title = tagLibFile.Tag.Title;
                if(title is not null)
                metadataDictionary.Add(TrackMetadataField.Title, title);

                string artists = SplitArtistsField(tagLibFile.Tag.Performers);
                if(!String.IsNullOrEmpty(artists))
                metadataDictionary.Add(TrackMetadataField.Artists, artists);

                var duration = tagLibFile.Properties.Duration;
                metadataDictionary.Add(TrackMetadataField.Duration, duration.TotalSeconds);

                int year = Convert.ToInt32(tagLibFile.Tag.Year);
                if (year != 0)
                {
                    metadataDictionary.Add(TrackMetadataField.Year, year);
                }
                String ISRC = tagLibFile.Tag.ISRC;
                if (!string.IsNullOrEmpty(ISRC))
                {
                    metadataDictionary.Add(TrackMetadataField.ISRC, ISRC);
                }

                String album = tagLibFile.Tag.Album;
                if (!String.IsNullOrEmpty(album))
                {
                    metadataDictionary.Add(TrackMetadataField.Album, album);
                }
                ProcessTrackImage(tagLibFile);



            }
            catch (TagLib.UnsupportedFormatException e)
            {
                throw new TrackUnsupportedFormatException();
            }
            catch (IOException e)
            {
                throw new TrackFileReadException("An error occurred while reading the track file.", e);
            }
            catch (TagLib.CorruptFileException e)
            {
                throw new TrackCorruptFileException("The track file is corrupt or has an unsupported format.", e);
            }
        }

        private void ProcessTrackImage(TagLib.File file)
        {
            if (file.Tag.Pictures.Length > 0 && ImagePath != null)
            {
                var picture = file.Tag.Pictures[0];
                var imageData = picture.Data.Data;
                var imageFormat = picture.MimeType;

                // Save the track image to disk
                var guid = Guid.NewGuid().ToString("N");
                var imageFileName = $"{guid}.jpg";
                var imageFilePath = Path.Combine(ImagePath, imageFileName);
                File.WriteAllBytes(imageFilePath, imageData);

                // Store the path to the saved image in the metadata dictionary
                metadataDictionary.Add(TrackMetadataField.Image, imageFileName);
            }
        }
        public static String SplitArtistsField(string[] input)
        {
            String output = String.Empty;
            if (input is not null)
            {
                if (input.Length == 1)
                {
                    //Split potentially item with more items
                    input = input[0].Split(splitTokens, StringSplitOptions.None);
                }
                output = String.Join(defaultArtistDelimiter, input);
            }
            return output;
        }

        public static ArtistTitleData GetTitleAndArtistFromPath(string path)
        {
            ArtistTitleData artistTile = new();
            var fileName = Path.GetFileNameWithoutExtension(path);
            var splitFileName = fileName.Split(" - ");

            if (splitFileName.Length == 1)
            {
                artistTile.Title = splitFileName[0];
            }
            else
            {
                artistTile.Artist = splitFileName[0];
                artistTile.Title = splitFileName[1];
            }
            return artistTile;
        }

        
    }
}
