using RA.Database.Models.Enums;
using RA.Logic.TrackFileLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.TrackFileLogic.Obsolet
{
    public class ProcessingTrackEventArgs : EventArgs
    {
        public ProcessingTrack Track { get; set; }

        public ProcessingTrackEventArgs(ProcessingTrack track)
        {
            Track = track;
        }
    }
    public interface ITrackFilesProcessor
    {
        public ProcessingTrack ProcessSingleItem(string path, bool readMetadata = false);

        public List<ProcessingTrack> ProcessItemsFromDirectory(string directoryPath,
            int categoryId, TrackType trackType = TrackType.Other, TrackStatus trackStatus = TrackStatus.Enabled,
            bool readMetadata = false);

        public delegate void TrackProcessedEventHandler(object sender, ProcessingTrackEventArgs e);

        /// <summary>
        /// Event raised when a track file is processed from a directory
        /// </summary>
        public event TrackProcessedEventHandler TrackProcessed;
    }
}
