using CommunityToolkit.Mvvm.ComponentModel;
using RA.DAL;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels.Templates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Templates
{
    public partial class PlannerManageTemplateViewModel : DialogViewModelBase
    {
        [ObservableProperty]
        private TemplateModel managedTemplate;
        private readonly ITemplatesService templatesService;

        #region Constructors
        public PlannerManageTemplateViewModel(IWindowService windowService, ITemplatesService templatesService) : base(windowService)
        {
            DialogName = "Add new template";
            ManagedTemplate = new();
            this.templatesService = templatesService;
        }

        public PlannerManageTemplateViewModel(IWindowService windowService, int templateId,
             ITemplatesService templatesService) : base(windowService)
        {
            DialogName = "Edit template";
            this.templatesService = templatesService;
            _ = LoadTemplate(templateId);
        }

        #endregion

        #region Data fetching
        private async Task LoadTemplate(int templateId)
        {
            var template = await templatesService.GetTemplate(templateId);
            ManagedTemplate = TemplateModel.FromDto(template);
        }
        #endregion

        protected override bool CanFinishDialog()
        {
            //TODO validate name not empty
            return true;
        }
    }
}
