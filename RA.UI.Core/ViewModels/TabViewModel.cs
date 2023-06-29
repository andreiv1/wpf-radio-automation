using Microsoft.Extensions.DependencyInjection;
using RA.UI.Core.Factories;
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

        private readonly IViewModelFactory viewModelFactory;
        private readonly Type viewModelType;

        public ViewModelBase ViewModel => viewModelFactory.CreateViewModel(viewModelType);

        public TabViewModel(IViewModelFactory viewModelFactory, string tabHeaderName, ImageSource tabIcon, Type viewModelType)
        {
            this.viewModelFactory = viewModelFactory;
            TabHeaderName = tabHeaderName;
            TabIcon = tabIcon;
            this.viewModelType = viewModelType;
        }
    }
}
