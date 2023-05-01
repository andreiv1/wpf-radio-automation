using RA.DAL;
using RA.Database.Models;
using RA.DTO;
using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using RA.UI.StationManagement.Components.Planner.ViewModels.Schedule.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Dialogs.TemplateSelectDialog
{
    public partial class TemplateSelectViewModel : DialogViewModelBase
    {
        private readonly ITemplatesService templatesService;

        public ObservableCollection<TemplateDto> Templates { get; private set; } = new();



        public TemplateSelectViewModel(IWindowService windowService, ITemplatesService templatesService) : base(windowService)
        {
            this.templatesService = templatesService;
            _ = LoadTemplates();
        }

        protected override bool CanFinishDialog()
        {
            return false;
        }

        #region Data fetching
        private async Task LoadTemplates()
        {
            var templates = await templatesService.GetTemplatesAsync();
            foreach(var template in templates)
            {
                Templates.Add(template);
            }
        }
        #endregion
    }
}
