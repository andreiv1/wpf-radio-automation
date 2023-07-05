using RA.DTO;
using RA.UI.Core;
using RA.UI.StationManagement.Components.MediaLibrary.ViewModels.MainContent;
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
    /// Interaction logic for CategoriesView.xaml
    /// </summary>
    public partial class CategoriesView : RAUserControl
    {
        public CategoriesView()
        {
            InitializeComponent();
        }

        private void SfDataGrid_CellDoubleTapped(object sender, Syncfusion.UI.Xaml.Grid.GridCellDoubleTappedEventArgs e)
        {
            var vm = DataContext as CategoriesViewModel;
            if (vm == null) return;
            vm.OpenCategoryCommand.Execute(null);
        }
    }
}
