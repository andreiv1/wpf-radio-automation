using RA.Database.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.Tracks
{
    public enum SubfolderScanOption
    {
        None,
        PutAllInSameCategory,
        CreateNewChildrenCategoryForEachExistingCategory
    }
    public class TrackFilesProcessorOptions
    {
        public string? DirectoryPath { get; internal set; }
        public int MainCategoryId { get; internal set; }
        public TrackType TrackType { get; internal set; } = TrackType.Other;
        public TrackStatus TrackStatus { get; internal set; } = TrackStatus.Enabled;
        public bool ReadMetadata { get; internal set; } = false;
        public bool ScanSubfolders { get; internal set; } = false;
        public SubfolderScanOption SubfolderScanOption { get; internal set; } = SubfolderScanOption.None;

        internal TrackFilesProcessorOptions() { }

    }
    public class TrackFilesProcessorOptionsBuilder
    {
        private readonly TrackFilesProcessorOptions options = new TrackFilesProcessorOptions();

        public TrackFilesProcessorOptionsBuilder(string directoryPath, int mainCategoryId)
        {
            options.DirectoryPath = directoryPath;
            options.MainCategoryId = mainCategoryId;
        }

        public TrackFilesProcessorOptionsBuilder SetTrackType(TrackType trackType)
        {
            options.TrackType = trackType;
            return this;
        }

        public TrackFilesProcessorOptionsBuilder SetTrackStatus(TrackStatus trackStatus)
        {
            options.TrackStatus = trackStatus;
            return this;
        }

        public TrackFilesProcessorOptionsBuilder SetReadMetadata(bool readMetadata)
        {
            options.ReadMetadata = readMetadata;
            return this;
        }

        public TrackFilesProcessorOptionsBuilder SetScanSubfolders(bool scan)
        {
            options.ScanSubfolders = scan;
            return this;
        }
        public TrackFilesProcessorOptionsBuilder SetSubfolderScanOption(SubfolderScanOption option)
        {
            options.SubfolderScanOption = option;
            return this;
        }

        public TrackFilesProcessorOptions Build()
        {
            return options;
        }
    }
}
