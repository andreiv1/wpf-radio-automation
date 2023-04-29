using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.UI.Core.Services;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RA.UI.StationManagement.Components.MediaLibrary.ViewModels.ImportItems
{
    public partial class ImportItemsSecondViewModel : ViewModelBase
    {
        private readonly IFolderBrowserDialogService folderBrowserDialog;
        public ImportItemsModel Model { get; set; }
        public ImportItemsSecondViewModel(IFolderBrowserDialogService folderBrowserDialog)
        {
            this.folderBrowserDialog = folderBrowserDialog;
        }

        [RelayCommand]
        private void PickFolder()
        {
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                Model.NewDestinationPath = folderBrowserDialog.SelectedPath;
              
            }
        }
    }
}
