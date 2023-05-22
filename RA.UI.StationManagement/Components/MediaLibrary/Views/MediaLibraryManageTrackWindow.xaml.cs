using RA.UI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
