using RA.Logic.TrackFileLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.TrackFileLogic
{
    public class TrackFileImporter : ITrackFilesImporter
    {
       public async Task<int> Import(IEnumerable<ProcessingTrack> processingTracks)
        {
            int numberOfTracks = 0;
            throw new NotImplementedException();
        }
    }
}
