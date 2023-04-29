using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent;
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

namespace RA.UI.StationManagement.Components.Planner.View.MainContent
{
    /// <summary>
    /// Interaction logic for PlannerScheduleView.xaml
    /// </summary>
    public partial class PlannerScheduleView : UserControl
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
