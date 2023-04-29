using RA.Logic.TrackFileLogic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.TrackFileLogic.Interfaces
{
    public class ArtistTitleData
    {
        public String? Artist { get; set; }
        public String? Title { get; set; }
    }
    public interface ITrackMetadataReader
    {
        public object? GetField(TrackMetadataField field);
        /// <summary>
        /// Extract from path Artist and Title
        /// The separator from path is " - "
        /// </summary>
        /// <param name="path">The file path</param>
        /// <returns>An object which is wrapping nullable Artist and Title fields</returns>
        public ArtistTitleData GetTitleAndArtistFromPath(string path);
    }
}
