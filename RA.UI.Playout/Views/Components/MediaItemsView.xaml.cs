using RA.UI.Core;
using RA.UI.Playout.ViewModels.Components;
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
    public partial class MediaItemsView : RAUserControl
    {
        public MediaItemsView()
        {
            InitializeComponent();
        }

        private void SfDataPager_OnDemandLoading(object sender, Syncfusion.UI.Xaml.Controls.DataPager.OnDemandLoadingEventArgs e)
        {
            var vm = DataContext as MediaItemsViewModel;
            vm?.LoadTracks(e.StartIndex, e.PageSize);
        }
    }
}
