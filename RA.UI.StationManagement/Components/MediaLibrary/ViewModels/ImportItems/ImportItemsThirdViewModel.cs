using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.Logic.Tracks.Enums;
using RA.Logic.Tracks.Models;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.MediaLibrary.ViewModels.ImportItems
{
    public partial class ImportItemsThirdViewModel : ViewModelBase
    {
        public ImportItemsModel Model { get; set; }

        
        public ImportItemsThirdViewModel()
        {
            Model?.Messages.Add("Started processing tracks...");
        }

        [RelayCommand]
        private void ExcludeSelectedFromImport()
        {
            var selected = Model.SelectedProcessingTrack;
            if (selected != null)
            {
               
                Model.TotalItems--;
                if (selected.Status == ProcessingTrackStatus.OK)
                {
                    Model.ValidItems--;
                }
                if(selected.Status == ProcessingTrackStatus.FAILED)
                {
                    Model.InvalidItems--;
                }
                if(selected.Status == ProcessingTrackStatus.WARNING)
                {
                    Model.WarningItems--;
                }
                Model?.ProcessingTracks.Remove(selected);
                Model.SelectedProcessingTrack = null;
               
            }
        }
    }
}
