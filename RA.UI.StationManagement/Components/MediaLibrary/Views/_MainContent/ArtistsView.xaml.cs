using RA.UI.Core;
using RA.UI.StationManagement.Components.MediaLibrary.Models;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.MainContent;
using Syncfusion.UI.Xaml.Grid;
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
            //var artist = e.Record as ArtistModel;
            //artist?.Tracks?.Clear();
            
        }

        private async void artistTracksGrid_DetailsViewExpanded(object sender, Syncfusion.UI.Xaml.Grid.GridDetailsViewExpandedEventArgs e)
        {
            var vm = DataContext as ArtistsViewModel;
            var artist = e.Record as ArtistModel;
            
            if (vm != null && artist != null)
            {
                await vm.LoadTracksForArtist(artist);
                e.DetailsViewItemsSource.Clear();
                e.DetailsViewItemsSource.Add("Tracks", artist.Tracks);
                var recordIndex = artistTracksGrid.ResolveToRecordIndex(artistTracksGrid.ResolveToRowIndex(e.Record));
                artistTracksGrid.CollapseDetailsViewAt(recordIndex);
                artistTracksGrid.ExpandDetailsViewAt(recordIndex);
            }
        }

        private void SfDataPager_OnDemandLoading(object sender, Syncfusion.UI.Xaml.Controls.DataPager.OnDemandLoadingEventArgs e)
        {
            ArtistsViewModel? vm = DataContext as ArtistsViewModel;
            vm?.LoadArtists(e.StartIndex, e.PageSize);
        }
    }
}
