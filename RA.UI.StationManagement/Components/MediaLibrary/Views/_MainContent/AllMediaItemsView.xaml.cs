using RA.UI.Core;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.MainContent;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RA.UI.StationManagement.Components.MediaLibrary.Views.MainContent
{
    public partial class AllMediaItemsView : RAUserControl
    {
        public AllMediaItemsView()
        {
            InitializeComponent();
            itemsSfDataGrid.SortColumnDescriptions.Clear();
        }
        private void SfDataPager_OnDemandLoading(object sender, Syncfusion.UI.Xaml.Controls.DataPager.OnDemandLoadingEventArgs e)
        {
            AllMediaItemsViewModel? vm = DataContext as AllMediaItemsViewModel;
            vm?.LoadTracks(e.StartIndex, e.PageSize);
        }
    }
}
