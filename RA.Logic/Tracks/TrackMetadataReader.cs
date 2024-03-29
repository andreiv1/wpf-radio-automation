﻿using NAudio.Wave;
using RA.Logic.Tracks.Enums;
using RA.Logic.Tracks.Exceptions;
using RA.Logic.Tracks.Models;
using System.Security.Cryptography;
using System.Text;

namespace RA.Logic.Tracks
{
    public class TrackMetadataReader : ITrackMetadataReader
    {
        /// <summary>
        /// Split tokens for multiple artists inside a field
        /// </summary>
        public static readonly string[] splitTokens = new string[] { ";", "; ", " ; ", "/", " / ", "/ " };

        public static readonly string defaultArtistDelimiter = " / ";
        private readonly string filePath;
        private Dictionary<TrackMetadataField, object> metadataDictionary;

        public static String? ImagePath { get; set; }

        static TrackMetadataReader()
        {
            CreateImageDirectoryPath();
        }
        
        public TrackMetadataReader(String filePath)
        {
            this.filePath = filePath;
            this.metadataDictionary = new();
            ReadMetadata();
        }

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
                string ISRC = tagLibFile.Tag.ISRC;
                if (!string.IsNullOrEmpty(ISRC))
                {
                    metadataDictionary.Add(TrackMetadataField.ISRC, ISRC);
                }

                string album = tagLibFile.Tag.Album;
                if (!string.IsNullOrEmpty(album))
                {
                    metadataDictionary.Add(TrackMetadataField.Album, album);
                }

                int bpm = Convert.ToInt32(tagLibFile.Tag.BeatsPerMinute);
                if(bpm != 0)
                {
                    metadataDictionary.Add(TrackMetadataField.BPM, bpm);
                }

                string genres = tagLibFile.Tag.JoinedGenres;
                if (!string.IsNullOrEmpty(genres))
                {
                    metadataDictionary.Add(TrackMetadataField.Genres, genres);
                }

                ProcessTrackImage(tagLibFile);
            }
            catch (TagLib.UnsupportedFormatException)
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
                using var md5 = MD5.Create();
                var picture = file.Tag.Pictures[0];
                var imageData = picture.Data.Data;
                var imageFormat = picture.MimeType;


                // Compute the hash of the imageData
                byte[] hashBytes = md5.ComputeHash(imageData);

                // Convert the hash bytes to a string representation
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                // The hash value as a string
                string hash = sb.ToString();


                var imageFileName = $"{hash}.jpg";

                var imageFilePath = Path.Combine(ImagePath, imageFileName);
                File.WriteAllBytes(imageFilePath, imageData);

                // Store the path to the saved image in the metadata dictionary
                metadataDictionary.Add(TrackMetadataField.Image, imageFileName);
            }
        }
        public static String SplitArtistsField(string[] input)
        {
            String output = String.Empty;

            if (input.Length == 1)
            {
                //Split potentially item with more items
                input = input[0].Split(splitTokens, StringSplitOptions.None);
            }
            output = String.Join(defaultArtistDelimiter, input);
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


        public static async Task<Dictionary<string, string>> GetAudioFileInfo(string audioFilePath)
        {
            Dictionary<string, string> fileInfo = new Dictionary<string, string>();

            await Task.Run(() =>
            {
                try
                {
                    using var file = TagLib.File.Create(audioFilePath);
                    fileInfo.Add("Bitrate", file.Properties.AudioBitrate.ToString());
                    fileInfo.Add("FileType", file.MimeType.Split("/")[1]);
                    fileInfo.Add("Frequency", file.Properties.AudioSampleRate.ToString());

                }
                catch (Exception)
                {

                }
            });

            return fileInfo;
        }

        private static void CreateImageDirectoryPath()
        {
            string roamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folderPath = Path.Combine(roamingPath, "RadioAutomation", "images");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                Console.WriteLine("Folder created successfully.");
            }
            else
            {
                Console.WriteLine("Folder already exists.");
            }
        }

    }
}
