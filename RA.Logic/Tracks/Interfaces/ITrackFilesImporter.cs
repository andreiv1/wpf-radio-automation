﻿using RA.Logic.Tracks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.Tracks
{
    public interface ITrackFilesImporter
    {
        Task<int> ImportAsync(IEnumerable<ProcessingTrack> processingTracks, TrackFilesProcessorOptions options);
        void Import(IEnumerable<ProcessingTrack> processingTracks, TrackFilesProcessorOptions options);
        Task ImportSingleAsync(ProcessingTrack processingTrack, TrackFilesProcessorOptions options);
    }
}
