using RA.UI.Playout.ViewModels.Controls;
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
    /// Interaction logic for MediaItemsView.xaml
    /// </summary>
    public partial class MediaItemsView : UserControl
    {
        public MediaItemsView()
        {
            InitializeComponent();
        }

        private void searchWatermarkTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MediaItemsViewModel? vm = DataContext as MediaItemsViewModel;
                if (vm is not null)
                {
                    vm.LoadItems();
                    e.Handled = true;
                }
            }
        }
    }
}
