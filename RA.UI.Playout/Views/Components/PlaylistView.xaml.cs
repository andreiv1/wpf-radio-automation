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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RA.UI.Playout.Views.Components
{
    /// <summary>
    /// Interaction logic for PlaylistView.xaml
    /// </summary>
    public partial class PlaylistView : UserControl
    {
        public PlaylistView()
        {
            InitializeComponent();
        }

        private void btnTopOfPlaylist_Click(object sender, RoutedEventArgs e)
        {
            if (playlistListBox.Items.Count > 0)
                playlistListBox.ScrollIntoView(playlistListBox.Items[0]);
        }

        private void btnBottomOfPlaylist_Click(object sender, RoutedEventArgs e)
        {
            if(playlistListBox.Items.Count > 0)
                playlistListBox.ScrollIntoView(playlistListBox.Items[playlistListBox.Items.Count - 1]);
        }
    }
}
