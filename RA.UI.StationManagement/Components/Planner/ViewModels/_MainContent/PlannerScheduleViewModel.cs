using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.View.Schedule;
using RA.UI.StationManagement.Components.Planner.ViewModels.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.MainContent
{
    public partial class PlannerScheduleViewModel : ViewModelBase
    {
        public ObservableCollection<TabViewModel> TabViewModels { get; private set; }
        public PlannerScheduleViewModel(PlannerScheduleCalendarViewModel plannerScheduleCalendarViewModel,
                                        PlannerDefaultScheduleViewModel plannerDefaultScheduleViewModel)
        {
            TabViewModels = new ObservableCollection<TabViewModel>();
            TabViewModels.Add(new TabViewModel("Calendar", (ImageSource)Application.Current.Resources["CalendarIcon"], plannerScheduleCalendarViewModel));
            TabViewModels.Add(new TabViewModel("Default schedules", (ImageSource)Application.Current.Resources["DefaultScheduleIcon"], plannerDefaultScheduleViewModel));
        }
    }
}
