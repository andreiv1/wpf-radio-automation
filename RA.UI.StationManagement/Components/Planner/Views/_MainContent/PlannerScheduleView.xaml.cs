using RA.UI.Core;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent;
using Syncfusion.Windows.Tools.Controls;
using System.Windows;


namespace RA.UI.StationManagement.Components.Planner.Views.MainContent
{
    public partial class PlannerScheduleView : RAUserControl
    {
        public PlannerScheduleView()
        {
            InitializeComponent();
        }

        private void tabControl_Loaded(object sender, RoutedEventArgs e)
        {
            var tabControl = sender as TabControlExt;
            if (tabControl != null)
            {
                tabControl.SelectedIndex = 0;
            }
        }
    }
}
