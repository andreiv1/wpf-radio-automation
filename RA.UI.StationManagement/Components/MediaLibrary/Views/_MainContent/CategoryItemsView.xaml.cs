using RA.DTO;
using RA.Logic;
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
    public partial class CategoryItemsView : RAUserControl
    {
        public CategoryItemsView()
        {
            InitializeComponent();
        }

        private void SfDataPager_OnDemandLoading(object sender, Syncfusion.UI.Xaml.Controls.DataPager.OnDemandLoadingEventArgs e)
        {
            var vm = DataContext as CategoryItemsViewModel as CategoryItemsViewModel;
            vm?.LoadTracksInCategory(e.StartIndex, e.PageSize);
        }
        private void subcategoriesSfDataGrid_CellDoubleTapped(object sender, Syncfusion.UI.Xaml.Grid.GridCellDoubleTappedEventArgs e)
        {
            CategoryItemsViewModel? vm = DataContext as CategoryItemsViewModel;
            if (vm == null) return;

            CategoryDTO? category = e.Record as CategoryDTO;
            vm.OpenSubcategoryCommand.Execute(null);
        }
    }
}
