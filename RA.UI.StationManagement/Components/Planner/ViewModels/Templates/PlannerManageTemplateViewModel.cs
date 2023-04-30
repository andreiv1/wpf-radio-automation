using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels.Templates
{
    public partial class PlannerManageTemplateViewModel : DialogViewModelBase
    {
        public PlannerManageTemplateViewModel(IWindowService windowService) : base(windowService)
        {
            DialogName = "Add new template";
        }

        public PlannerManageTemplateViewModel(IWindowService windowService, int templateId) : base(windowService)
        {
            DialogName = "Edit template";
            throw new NotImplementedException();
        }

        protected override bool CanFinishDialog()
        {
            return false;
        }
    }
}
