using RA.UI.Core;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent;
using RA.UI.StationManagement.Components.Planner.ViewModels.Schedule;
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

namespace RA.UI.StationManagement.Components.Planner.View.Schedule
{
    /// <summary>
    /// Interaction logic for PlannerScheduleCalendarView.xaml
    /// </summary>
    public partial class PlannerScheduleCalendarView : RAUserControl
    {
        public PlannerScheduleCalendarView()
        {
            InitializeComponent(); 
            DataContextChanged += PlannerScheduleCalendarView_DataContextChanged;
        }

        private void PlannerScheduleCalendarView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var dataContext = DataContext as PlannerScheduleCalendarViewModel;
            calendar.ItemsSource = dataContext?.CalendarItems;
        }
    }
}
