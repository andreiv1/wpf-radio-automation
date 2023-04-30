using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RA.DAL;
using RA.DTO;
using RA.UI.Core.Services;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels.MainContent.Models;
using RA.UI.StationManagement.Components.Planner.ViewModels.Templates;
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

        public ObservableCollection<TemplateDto> Templates { get; set; } = new();
        public ObservableCollection<TemplateClockItemModel> ClocksForSelectedTemplate { get; set; } = new();

        [ObservableProperty]
        private TemplateDto? selectedTemplate = null;

        partial void OnSelectedTemplateChanged(TemplateDto? value)
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

        #region Commands
        [RelayCommand]
        private void AddTemplate()
        {
            windowService.ShowDialog<PlannerManageTemplateViewModel>();
        }
        #endregion
    }
}
