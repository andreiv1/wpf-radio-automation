using RA.Logic;
using RA.UI.Core;
using Syncfusion.Windows.Controls.Input;
using System.Windows.Input;

namespace RA.UI.Playout.Views.Components
{
    public partial class NowPlayingView : RAUserControl
    {
        public NowPlayingView()
        {
            InitializeComponent();
        }

        private void SfRangeSlider_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            double selectedPositionInSeconds = ((SfRangeSlider)sender).Value;
            var vm = (ViewModels.Components.NowPlayingViewModel)DataContext;
            vm?.SeekTo(selectedPositionInSeconds);
            DebugHelper.WriteLine(this, $"{selectedPositionInSeconds}");
        }
    }
}
