using RA.UI.Core;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent.Models;
using Syncfusion.UI.Xaml.Grid;
using System.Windows;
using System.Windows.Input;

namespace RA.UI.StationManagement.Components.Planner.Views.MainContent
{
    public partial class PlannerClocksView : RAUserControl
    {
        public PlannerClocksView()
        {
            InitializeComponent();
        }

        private void selectedClockItemsDataGrid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.D)
            {
                // Deselect all items
                selectedClockItemsDataGrid.SelectedItems.Clear();
            }
        }
    }
}
