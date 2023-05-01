using RA.UI.Core.Themes;
using Syncfusion.SfSkinManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RA.UI.Core
{
    public partial class RAUserControl : UserControl, IDisposable
    {
        public RAUserControl() : base()
        {
            ThemeManager.ApplyTheme(this);
            Unloaded += RAUserControl_Unloaded;
        }

        public void Dispose()
        {
            Unloaded -= RAUserControl_Unloaded;
        }

        private void RAUserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            SfSkinManager.Dispose(this);
        }
    }
}
