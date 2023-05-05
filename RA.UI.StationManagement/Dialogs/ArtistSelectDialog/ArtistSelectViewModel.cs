using RA.UI.Core.Services.Interfaces;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Dialogs.ArtistSelectDialog
{
    public partial class ArtistSelectViewModel : DialogViewModelBase
    {
        public ArtistSelectViewModel(IWindowService windowService) : base(windowService)
        {
        }

        protected override bool CanFinishDialog()
        {
            return false;
        }
    }
}
