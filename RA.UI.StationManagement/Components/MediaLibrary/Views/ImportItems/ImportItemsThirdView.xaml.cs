using RA.UI.Core;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.ImportItems;
using Syncfusion.UI.Xaml.Grid.Helpers;
using Syncfusion.UI.Xaml.ScrollAxis;
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

namespace RA.UI.StationManagement.Components.MediaLibrary.Views.ImportItems
{
    /// <summary>
    /// Interaction logic for ImportItemsThirdView.xaml
    /// </summary>
    public partial class ImportItemsThirdView : RAUserControl
    {
        public ImportItemsThirdView()
        {
            InitializeComponent();
            
        }

        private void ProgressBar_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ImportItemsThirdViewModel viewModel)
            {
                progressBar.Visibility = ((ImportItemsThirdViewModel)viewModel).Model.IsTrackProcessRunning
                    ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
