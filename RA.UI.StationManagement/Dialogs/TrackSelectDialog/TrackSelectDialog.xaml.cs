using RA.UI.Core;


namespace RA.UI.StationManagement.Dialogs.TrackSelectDialog
{
    public partial class TrackSelectDialog : RAWindow
    {
        public TrackSelectDialog()
        {
            InitializeComponent();
        }

        private void SfDataPager_OnDemandLoading(object sender, Syncfusion.UI.Xaml.Controls.DataPager.OnDemandLoadingEventArgs e)
        {
            var vm = DataContext as TrackSelectViewModel;
            vm?.LoadTracks(e.StartIndex, e.PageSize);
        }
    }
}
