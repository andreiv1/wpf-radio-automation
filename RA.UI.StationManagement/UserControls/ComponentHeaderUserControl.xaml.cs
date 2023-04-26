using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RA.UI.StationManagement.UserControls
{
    /// <summary>
    /// Interaction logic for ComponentHeaderUserControl.xaml
    /// </summary>
    public partial class ComponentHeaderUserControl : UserControl
    {
        public static readonly DependencyProperty HeaderTextProperty =
        DependencyProperty.Register("HeaderText", typeof(string), typeof(ComponentHeaderUserControl));

        public static readonly DependencyProperty HeaderDescriptionProperty =
        DependencyProperty.Register("Description", typeof(string), typeof(ComponentHeaderUserControl));

        public static readonly DependencyProperty BackgroundColorProperty =
       DependencyProperty.Register("BackgroundColor", typeof(Brush), typeof(ComponentHeaderUserControl));
        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

       
        public string Description
        {
            get { return (string)GetValue(HeaderDescriptionProperty); }
            set { SetValue(HeaderDescriptionProperty, value); }
        }

        public Brush BackgroundColor
        {
            get { return (Brush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }
        public ComponentHeaderUserControl()
        {
            InitializeComponent();
        }
    }
}
