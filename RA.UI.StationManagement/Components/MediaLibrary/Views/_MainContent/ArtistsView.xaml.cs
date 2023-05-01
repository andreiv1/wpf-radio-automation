using RA.UI.Core;
using RA.UI.StationManagement.Components.MediaLibrary.Models;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.MainContent;
using System.Windows.Input;

namespace RA.UI.StationManagement.Components.MediaLibrary.Views.MainContent
{
    public partial class ArtistsView : RAUserControl
    {
        public ArtistsView()
        {
            InitializeComponent();
        }

        private void searchWatermarkTextbox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void artistTracksGrid_DetailsViewCollapsed(object sender, Syncfusion.UI.Xaml.Grid.GridDetailsViewCollapsedEventArgs e)
        {
            var artist = e.Record as ArtistModel;
            artist?.Tracks?.Clear();
            
        }

        private async void artistTracksGrid_DetailsViewExpanded(object sender, Syncfusion.UI.Xaml.Grid.GridDetailsViewExpandedEventArgs e)
        {
            var vm = DataContext as ArtistsViewModel;
            var artist = e.Record as ArtistModel;
            e.DetailsViewItemsSource.Clear();
            if (artist != null)
            {
                await vm?.LoadTracksForArtist(artist);
                e.DetailsViewItemsSource.Add("Tracks", artist.Tracks);
            }
        }
    }
}
