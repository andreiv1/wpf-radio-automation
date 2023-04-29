using CommunityToolkit.Mvvm.ComponentModel;
using RA.DAL.Interfaces;
using RA.Dto;
using RA.UI.Core.Services;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.MainContent
{
    public partial class PlannerDayTemplatesViewModel : ViewModelBase
    {
        private readonly ITemplatesService templatesService;
        private readonly IClocksService clocksService;
        private readonly IDispatcherService dispatcherService;

        public ObservableCollection<TemplateDto> Templates { get; set; } = new();
        public ObservableCollection<TemplateClockItemModel> ClocksForSelectedTemplate { get; set; } = new();

        [ObservableProperty]
        private TemplateDto? selectedTemplate = null;

        partial void OnSelectedTemplateChanged(TemplateDto? value)
        {
            _ = LoadClocksForSelectedTemplate();
        }

        public PlannerDayTemplatesViewModel(ITemplatesService templatesService, 
            IDispatcherService dispatcherService)
        {
            this.templatesService = templatesService;
            this.clocksService = clocksService;
            this.dispatcherService = dispatcherService;

            _ = LoadTemplates();
        }

        private async Task LoadTemplates()
        {
            var templates = await templatesService.GetTemplatesAsync();
            Templates.Clear();
            foreach (var template in templates)
            {
                dispatcherService.InvokeOnUIThread(() =>
                {
                    Templates.Add(template);
                });
            }
        }

        private async Task LoadClocksForSelectedTemplate()
        {
            var items = await templatesService.GetTemplatesForClockWithId(selectedTemplate.Id);
            ClocksForSelectedTemplate.Clear();
            foreach (var item in items)
            {
                dispatcherService.InvokeOnUIThread(() =>
                {
                    ClocksForSelectedTemplate.Add(TemplateClockItemModel.FromDto(item));
                });
            }
        }

        #region Test data
        private void LoadTestData()
        {
            ClocksForSelectedTemplate.Add(new TemplateClockItemModel()
            {
                ClockName = "Test",
                ClockSpan = 12,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(4),
            });

            ClocksForSelectedTemplate.Add(new TemplateClockItemModel()
            {
                ClockName = "Test",
                ClockSpan = 12,
                StartTime = DateTime.Now.AddHours(4),
                EndTime = DateTime.Now.AddHours(8),
            });
        }
        #endregion
    }
}
