using RA.DTO;
using RA.Logic;
using RA.UI.Core;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent;
using Syncfusion.UI.Xaml.Scheduler;
using System;
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
            //e.Effects = DragDropEffects.Copy;
            //e.Handled = true;
        }

        private void selectedTemplateScheduler_Drop(object sender, DragEventArgs e)
        {
            DebugHelper.WriteLine(this, $"Trying to handle something to drop.");
            if (e.Data.GetDataPresent(typeof(ClockDTO)))
            {
                e.Effects = DragDropEffects.Copy;
                ClockDTO? clock = e.Data.GetData(typeof(ClockDTO)) as ClockDTO;
                if (clock == null) return;
                DebugHelper.WriteLine(this, $"Dropped clock: {clock.Id} - {clock.Name}");
            }
        }

        private DateTime droppingTime;
        private void selectedTemplateScheduler_AppointmentDropping(object sender, AppointmentDroppingEventArgs e)
        {
            droppingTime = e.DropTime;
            DebugHelper.WriteLine(this, droppingTime.ToString("HH:mm"));
        }
    }
}
