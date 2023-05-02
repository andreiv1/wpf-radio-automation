using CommunityToolkit.Mvvm.ComponentModel;
using RA.DAL;
using RA.Database.Models;
using RA.DTO;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Schedule
{
    public partial class PlannerManageScheduleItemViewModel : DialogViewModelBase
    {
        [ObservableProperty]
        private SchedulePlannedType scheduleType = SchedulePlannedType.Recurrent;

        private static IEnumerable<SchedulePlannedFrequency> scheduleFrequencies = (IEnumerable<SchedulePlannedFrequency>)
            Enum.GetValues(typeof(SchedulePlannedFrequency)).Cast<SchedulePlannedType>();
        private readonly IDispatcherService dispatcherService;
        private readonly ITemplatesService templatesService;

        public List<SchedulePlannedFrequency> ScheduleFrequencies => scheduleFrequencies.ToList();

        public ObservableCollection<TemplateDto> Templates { get; set; } = new();

        public PlannerManageScheduleItemViewModel(IWindowService windowService, IDispatcherService dispatcherService,
            ITemplatesService templatesService) : base(windowService)
        {
            this.dispatcherService = dispatcherService;
            this.templatesService = templatesService;
            _ = LoadTemplates();
        }

        private async Task LoadTemplates()
        {
            var templates = await Task.Run(() => templatesService.GetTemplatesAsync());
            Templates.Clear();
            foreach (var template in templates)
            {
                dispatcherService.InvokeOnUIThread(() => Templates.Add(template));
            }
        }

        protected override void FinishDialog()
        {
            MessageBox.Show($"Selected schedule type: {ScheduleType.ToString()}");
            base.FinishDialog();
        }

        protected override bool CanFinishDialog()
        {
            return true;
        }
    }
}
