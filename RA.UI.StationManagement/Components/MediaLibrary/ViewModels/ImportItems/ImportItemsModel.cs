using CommunityToolkit.Mvvm.ComponentModel;
using RA.Database.Models.Enums;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using RA.Database;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using RA.Logic.Tracks.Models;

namespace RA.UI.StationManagement.Components.MediaLibrary.ViewModels.Models
{
    public enum CompleteScanOptions
    {
        None,
        PutItemsInTheSameCategory,
        CreateNewCategoriesAndAsignItems
    }
    public enum DestinationOptions
    {
        LeaveCurrent,
        CopyToANewLocation,
        MoveToANewLocation
    }
    public partial class ImportItemsModel : ObservableValidator
    {
        #region Constructor
        public ImportItemsModel()
        {
           
        }
        #endregion
        #region First View

        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Folder path is required.")]
        [ObservableProperty]
        private string? folderPath;

        [NotifyDataErrorInfo]
        [Required]
        [ObservableProperty]
        private CategoryHierarchyDTO? selectedCategory;

        [ObservableProperty]
        private string? categorySearchQuery = null;
        public ObservableCollection<CategoryHierarchyDTO> Categories { get; set; } = new();

        public static List<string> TrackTypes => Enum.GetNames(typeof(TrackType)).ToList();
        public static List<string> TrackStatuses => Enum.GetNames(typeof(TrackStatus)).ToList();

        [ObservableProperty]
        private TrackType selectedTrackType = TrackType.Song;

        [ObservableProperty]
        private TrackStatus selectedTrackStatus = TrackStatus.Enabled;

        [ObservableProperty]
        private bool isCompleteScan = false;

        [ObservableProperty]
        private CompleteScanOptions scanOptions = CompleteScanOptions.None;

        partial void OnIsCompleteScanChanged(bool value)
        {
            if(!value)
            {
                ScanOptions = CompleteScanOptions.None;
            }
        }

        [ObservableProperty]
        private bool analyseTrackMarkers = false;

        [ObservableProperty]
        private bool readItemsMetadata = true;
        #endregion

        #region Second View
        [ObservableProperty]
        private DestinationOptions destinationOption = DestinationOptions.LeaveCurrent;

        partial void OnDestinationOptionChanged(DestinationOptions value)
        {
            if(value == DestinationOptions.LeaveCurrent)
            {
                NewDestinationPath = null;
                IsLeaveCurrentSelected = true;
            } else
            {
                IsLeaveCurrentSelected = false;
            }
        }

        [ObservableProperty]
        private String? newDestinationPath = null;

        [ObservableProperty]
        private bool isLeaveCurrentSelected = true;

        partial void OnIsLeaveCurrentSelectedChanged(bool value)
        {
            if (!value)
            {
                NewDestinationPath = null;
            }
        }
        #endregion

        #region Third View
        [ObservableProperty]
        private bool isTrackProcessRunning = false;
        public ObservableCollection<ProcessingTrack> ProcessingTracks { get; set; } = new();

        [ObservableProperty]
        private ProcessingTrack? selectedProcessingTrack;
        public ObservableCollection<string> Messages { get; set; } = new();

        [ObservableProperty]
        private int totalItems = 0;
        [ObservableProperty]
        private int validItems = 0;
        [ObservableProperty]
        private int invalidItems = 0;
        [ObservableProperty]
        private int warningItems = 0;

        [ObservableProperty]
        private int processedItems = 0;
        #endregion
    }
}
