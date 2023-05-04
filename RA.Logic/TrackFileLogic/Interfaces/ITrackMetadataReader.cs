using RA.Logic.TrackFileLogic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.TrackFileLogic
{
    public class ArtistTitleData
    {
        public String? Artist { get; set; }
        public String? Title { get; set; }
    }
    public interface ITrackMetadataReader
    {
        public object? GetField(TrackMetadataField field);
    }
}
