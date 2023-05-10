using RA.UI.Core;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels;
using Syncfusion.Windows.Controls.RichTextBoxAdv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RA.UI.StationManagement.Components.MediaLibrary.Views
{
    /// <summary>
    /// Interaction logic for MediaLibraryManageTrackWindow.xaml
    /// </summary>
    public partial class MediaLibraryManageTrackWindow : RAWindow
    {
        public MediaLibraryManageTrackWindow()
        {
            InitializeComponent();
 
        }



        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            soundwaveComponent.Play();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            soundwaveComponent.Pause();
        }

        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MediaLibraryManageTrackViewModel;
            if (vm == null) return;
            var markers = await soundwaveComponent.GetMarkers();
            vm.Track!.StartCue = markers[0];
            vm.Track!.NextCue = markers[1];
            vm.Track!.EndCue = markers[2];

            vm.SaveTrackCommand.Execute(null);
        }

     
    }
}
