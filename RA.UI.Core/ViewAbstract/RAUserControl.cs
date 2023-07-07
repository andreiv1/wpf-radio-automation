using RA.UI.Core.Themes;
using Syncfusion.SfSkinManager;
using System;
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
            ToDispose();
            Unloaded -= RAUserControl_Unloaded;
        }

        protected virtual void ToDispose()
        {

        }

        private void RAUserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            SfSkinManager.Dispose(this);
        }
    }
}
