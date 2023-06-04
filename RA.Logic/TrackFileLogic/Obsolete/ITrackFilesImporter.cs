using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RA.Logic.TrackFileLogic.Models;

namespace RA.Logic.TrackFileLogic.Obsolet
{
    public interface ITrackFilesImporter
    {
        public Task<bool> Import(IEnumerable<ProcessingTrack> processingTracks);
    }
}
