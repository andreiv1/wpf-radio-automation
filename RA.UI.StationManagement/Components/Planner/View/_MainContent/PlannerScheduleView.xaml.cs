using RA.UI.Core;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent;
using System.Windows;


namespace RA.UI.StationManagement.Components.Planner.View.MainContent
{
    public partial class PlannerScheduleView : RAUserControl
    {
        public PlannerScheduleView()
        {
            InitializeComponent();
            DataContextChanged += PlannerScheduleView_DataContextChanged;
        }

        private void PlannerScheduleView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var dataContext = DataContext as PlannerScheduleViewModel;
        }
    }
}
