using Microsoft.Extensions.DependencyInjection;
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

        private readonly ViewModelBase viewModel;
        public ViewModelBase ViewModel => viewModel;

        public TabViewModel(string tabHeaderName, ImageSource tabIcon, ViewModelBase viewModel)
        {
            TabHeaderName = tabHeaderName;
            TabIcon = tabIcon;
            this.viewModel = viewModel;
        }
    }
}
