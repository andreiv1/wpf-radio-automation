using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DTO;
using RA.Logic;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent.Models;
using RA.UI.StationManagement.Components.Planner.ViewModels.Templates;
using RA.UI.StationManagement.Components.Planner.ViewModels.Templates.Models;
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
        private readonly IWindowService windowService;
        private readonly ITemplatesService templatesService;
        private readonly IDispatcherService dispatcherService;

        public ObservableCollection<TemplateDTO> Templates { get; set; } = new();
        public ObservableCollection<TemplateClockItemModel> ClocksForSelectedTemplate { get; set; } = new();

        [ObservableProperty]
        private TemplateDTO? selectedTemplate = null;

        partial void OnSelectedTemplateChanged(TemplateDTO? value)
        {
            _ = LoadClocksForSelectedTemplate();
        }

        public PlannerDayTemplatesViewModel(IWindowService windowService, ITemplatesService templatesService, 
            IDispatcherService dispatcherService)
        {
            this.windowService = windowService;
            this.templatesService = templatesService;
            this.dispatcherService = dispatcherService;

            _ = LoadTemplates();
        }

        #region Data fetching
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
            if (SelectedTemplate == null) return; 
            var items = await templatesService.GetTemplatesForClockWithId(SelectedTemplate.Id);
            ClocksForSelectedTemplate.Clear();
            foreach (var item in items)
            {
                dispatcherService.InvokeOnUIThread(() =>
                {
                    ClocksForSelectedTemplate.Add(TemplateClockItemModel.FromDto(item));
                });
            }
        }

        #endregion

        public async Task AddClockToTemplate(int clockId, TimeSpan start, int span)
        {
            if (SelectedTemplate == null) return;
            var clockTemplate = new ClockTemplateDTO()
            {
                ClockId = clockId,
                StartTime = start,
                ClockSpan = span,
                TemplateId = SelectedTemplate.Id,
            };
            await templatesService.AddClockToTemplate(clockTemplate);
            _ = LoadClocksForSelectedTemplate();
        }

        #region Commands
        [RelayCommand]
        private void AddTemplateDialog()
        {
            var result = windowService.ShowDialog<PlannerManageTemplateViewModel>();
            TemplateModel template = result.ManagedTemplate;
            templatesService.AddTemplate(TemplateModel.ToDto(template));

            _ = LoadTemplates();
        }

        [RelayCommand]
        private void EditTemplateDialog()
        {
            if (SelectedTemplate == null) return;
            var result = windowService.ShowDialog<PlannerManageTemplateViewModel>(SelectedTemplate.Id);
            TemplateModel template = result.ManagedTemplate;
            templatesService.UpdateTemplate(TemplateModel.ToDto(template));
            _ = LoadTemplates();
        }

        [RelayCommand]
        private void DeleteTemplateDialog()
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        private void DuplicateTemplateDialog()
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        private void RefreshTemplates()
        {
            _ = LoadTemplates();
        }

        [RelayCommand]
        private void InsertClock()
        {
            windowService.ShowWindow<PlannerTemplateSelectClockViewModel>();
        }
        #endregion
    }
}
