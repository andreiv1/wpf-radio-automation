using Microsoft.Extensions.DependencyInjection;
using RA.UI.Core.Factories;
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
        private readonly IViewModelFactory viewModelFactory;

        public ObservableCollection<TabViewModel> TabViewModels { get; set; }

        public PlannerMainViewModel(IViewModelFactory viewModelFactory)
        {
            this.viewModelFactory = viewModelFactory;
            TabViewModels = new ObservableCollection<TabViewModel>()
            {
                new TabViewModel(viewModelFactory, "Clocks", (ImageSource)Application.Current.Resources["ClockIcon"], typeof(PlannerClocksViewModel)),
                new TabViewModel(viewModelFactory, "Day Templates", (ImageSource)Application.Current.Resources["TemplateIcon"], typeof(PlannerDayTemplatesViewModel)),
                new TabViewModel(viewModelFactory, "Schedule", (ImageSource)Application.Current.Resources["ScheduleIcon"], typeof(PlannerScheduleViewModel)),
                new TabViewModel(viewModelFactory, "Playlists", (ImageSource)Application.Current.Resources["PlaylistIcon"], typeof(PlannerPlaylistsViewModel)),
            };
        }
    }
}
