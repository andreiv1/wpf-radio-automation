using RA.DTO;
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
    public partial class CategoriesView : UserControl
    {
        public CategoriesView()
        {
            InitializeComponent();
        }

        private void CustomDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                //if (dataGrid.SelectedItem is CategoryDto category)
                //{
                //    var vm = DataContext as CategoriesViewModel;
                //    var subcategoryId = category.Id;
                //    if(subcategoryId.HasValue)
                //    {
                //        vm?.NavigateToSubcategory(subcategoryId.Value);
                //    }
                    
                //}
            }
        }

        private void searchWatermarkTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //CategoriesViewModel? vm = DataContext as CategoriesViewModel;
                //if (vm is not null)
                //{
                //    throw new NotImplementedException();
                //    e.Handled = true;
                //}
            }
        }
    }
}
