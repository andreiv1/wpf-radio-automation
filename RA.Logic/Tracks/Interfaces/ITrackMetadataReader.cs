using RA.Logic.Tracks.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.Tracks
{
    
    public interface ITrackMetadataReader
    {
        public object? GetField(TrackMetadataField field);
    }
}
