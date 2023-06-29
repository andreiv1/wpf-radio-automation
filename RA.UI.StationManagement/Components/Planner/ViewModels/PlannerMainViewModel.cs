using Microsoft.Extensions.DependencyInjection;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RA.UI.StationManagement.Components.Planner.ViewModels
{
    public partial class PlannerMainViewModel : ViewModelBase
    {
        public ObservableCollection<TabViewModel> TabViewModels { get; set; }
        public PlannerMainViewModel(PlannerClocksViewModel plannerClocksViewModel,
                                    PlannerDayTemplatesViewModel plannerDayTemplatesViewModel,
                                    PlannerScheduleViewModel plannerScheduleViewModel,
                                    PlannerPlaylistsViewModel plannerPlaylistsViewModel)
        {
            TabViewModels = new ObservableCollection<TabViewModel>();
            TabViewModels.Add(new TabViewModel("Clocks", (ImageSource)Application.Current.Resources["ClockIcon"], plannerClocksViewModel));
            TabViewModels.Add(new TabViewModel("Day Templates", (ImageSource)Application.Current.Resources["TemplateIcon"], plannerDayTemplatesViewModel));
            TabViewModels.Add(new TabViewModel("Schedule", (ImageSource)Application.Current.Resources["ScheduleIcon"], plannerScheduleViewModel));
            TabViewModels.Add(new TabViewModel("Playlists", (ImageSource)Application.Current.Resources["PlaylistIcon"], plannerPlaylistsViewModel));
            
        }
    }
}
