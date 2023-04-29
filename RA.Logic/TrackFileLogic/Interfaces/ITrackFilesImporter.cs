using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.TrackFileLogic.Interfaces
{
    public interface ITrackFilesImporter
    {
        public int Import(List<ProcessingTrack> processingTracks);
    }
}
