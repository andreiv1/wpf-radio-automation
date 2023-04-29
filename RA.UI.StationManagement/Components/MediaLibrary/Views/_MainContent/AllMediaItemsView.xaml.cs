using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.MainContent;
using Syncfusion.SfSkinManager;
using Syncfusion.Themes.Windows11Light.WPF;
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

namespace RA.UI.StationManagement.Components.MediaLibrary.Views.MainContent
{
    /// <summary>
    /// Interaction logic for AllMediaItemsView.xaml
    /// </summary>
    public partial class AllMediaItemsView : UserControl
    {
        public AllMediaItemsView()
        {
            InitializeComponent();
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
