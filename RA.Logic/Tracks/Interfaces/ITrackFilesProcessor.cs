﻿using RA.Logic.Tracks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.Tracks
{
    public interface ITrackFilesProcessor
    {
        Task<int> CountItemsInDirectoryAsync(TrackFilesProcessorOptions options);
        IAsyncEnumerable<ProcessingTrack> ProcessItemsFromDirectoryAsync(TrackFilesProcessorOptions options);
        ProcessingTrack ProcessSingleItem(string path, bool readMetadata = false);
        Task<ProcessingTrack> ProcessSingleItemAsync(string path, bool readMetadata = false);
    }
}