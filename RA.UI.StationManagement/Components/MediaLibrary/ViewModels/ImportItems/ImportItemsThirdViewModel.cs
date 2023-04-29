using CommunityToolkit.Mvvm.ComponentModel;
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
    }
}
