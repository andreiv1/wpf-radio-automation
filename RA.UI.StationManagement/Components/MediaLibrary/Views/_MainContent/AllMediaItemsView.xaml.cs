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


        private void searchWatermarkTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //AllMediaItemsViewModel? vm = DataContext as AllMediaItemsViewModel;
                //if (vm is not null)
                //{
                //    vm.LoadItems();
                //    e.Handled = true;
                //}
            }
        }

        private void searchTextbox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Visual originalSource && originalSource is Button clearButton)
            {
                //AllMediaItemsViewModel? vm = DataContext as AllMediaItemsViewModel;
                //if (vm is not null)
                //{
                //    searchTextbox.Text = "";
                //    vm.LoadItems();
                //    e.Handled = true;
                //}
            }
        }

        private void SfDataPager_OnDemandLoading(object sender, Syncfusion.UI.Xaml.Controls.DataPager.OnDemandLoadingEventArgs e)
        {
            AllMediaItemsViewModel? vm = DataContext as AllMediaItemsViewModel;
            vm?.LoadTracks(e.StartIndex, e.PageSize);
        }
    }
}
