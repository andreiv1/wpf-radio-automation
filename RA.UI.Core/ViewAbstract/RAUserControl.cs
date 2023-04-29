using Syncfusion.SfSkinManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RA.UI.Core.ViewAbstract
{
    public partial class RAUserControl : UserControl
    {
        public RAUserControl() : base()
        {
            SfSkinManager.SetTheme(this, new Theme("Windows11Light"));
        }
    }
}
