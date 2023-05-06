using RA.DTO;
using RA.Logic;
using RA.UI.Core;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent;
using Syncfusion.UI.Xaml.Scheduler;
using System;
using System.Threading.Tasks;
using System.Windows;


namespace RA.UI.StationManagement.Components.Planner.Views.MainContent
{
    public partial class PlannerDayTemplatesView : RAUserControl
    {
        public PlannerDayTemplatesView()
        {
            InitializeComponent();
            DebugHelper.WriteLine(this, $"SCHEDULER ITEMS: this.selectedTemplateScheduler.ItemsSource");
            DataContextChanged += PlannerDayTemplatesView_DataContextChanged;
        }

        private void PlannerDayTemplatesView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DebugHelper.WriteLine(this, $"Data context changed");
            var dataContext = DataContext as PlannerDayTemplatesViewModel;
            selectedTemplateScheduler.ItemsSource = dataContext?.ClocksForSelectedTemplate;
        }

        private void selectedTemplateScheduler_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ClockDTO)))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private async void selectedTemplateScheduler_Drop(object sender, DragEventArgs e)
        {
            DebugHelper.WriteLine(this, $"Trying to handle something to drop.");
            if (e.Data.GetDataPresent(typeof(ClockDTO)))
            {
                e.Effects = DragDropEffects.Copy;
                ClockDTO? clock = e.Data.GetData(typeof(ClockDTO)) as ClockDTO;
                if (clock == null) return;
                DebugHelper.WriteLine(this, $"Dropped clock: {clock.Id} - {clock.Name}");
                DebugHelper.WriteLine(this, $"At HOUR {droppingTime.Hour}");
                DebugHelper.WriteLine(this, $"Full: {droppingTime.ToString("HH:mm")}");
                var vm = DataContext as PlannerDayTemplatesViewModel;
                if(vm != null && clock != null && clock.Id != null)
                {
                    await vm.AddClockToTemplate(clock.Id.Value, new TimeSpan(droppingTime.Hour, 0, 0), 1);
                }
                e.Handled = true;
            }
        }

        private DateTime droppingTime;
        private void selectedTemplateScheduler_AppointmentDropping(object sender, AppointmentDroppingEventArgs e)
        {
            droppingTime = e.DropTime;
        }
    }
}
