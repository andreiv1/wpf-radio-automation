using RA.UI.Core.Themes;
using RA.UI.Core.ViewModels;
using Syncfusion.SfSkinManager;
using Syncfusion.Themes.Windows11Light.WPF;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RA.UI.Core
{
    public partial class RAWindow : ChromelessWindow
    {
        public RAWindow() : base()
        {
            ThemeManager.ApplyTheme(this);
            this.ResizeBorderThickness = new Thickness(0);
            this.CornerRadius = new CornerRadius(0);
        }
    }
}
