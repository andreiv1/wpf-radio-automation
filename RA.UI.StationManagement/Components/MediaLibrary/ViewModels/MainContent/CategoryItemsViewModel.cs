using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.MediaLibrary.ViewModels.MainContent
{
    public partial class CategoryItemsViewModel : ViewModelBase
    {
        private readonly int categoryId;

        public CategoryItemsViewModel(int categoryId)
        {
            this.categoryId = categoryId;
        }
    }
}
