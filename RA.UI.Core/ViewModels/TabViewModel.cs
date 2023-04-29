using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RA.UI.Core.ViewModels
{
    public partial class TabViewModel : ViewModelBase
    {
        public string TabHeaderName { get; set; }
        public ImageSource TabIcon { get; set; }
        private readonly Type viewModelType;

        private readonly IServiceProvider serviceProvider;
        public ViewModelBase ViewModel
        {
            get
            {
                return (ViewModelBase)serviceProvider.GetService(viewModelType);
            }
        }

        public TabViewModel(IServiceProvider serviceProvider, Type viewModelType)
        {
            this.serviceProvider = serviceProvider;
            this.viewModelType = viewModelType;
        }
    }
}
