using RA.Logic.TrackFileLogic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.TrackFileLogic
{
    
    public interface ITrackMetadataReader
    {
        public object? GetField(TrackMetadataField field);
    }
}
