using Microsoft.Extensions.DependencyInjection;
using RA.Logic.AudioPlayer.Interfaces;
using RA.UI.Core;
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

namespace RA.UI.Playout.Views.Components
{
    public partial class NowPlayingView : RAUserControl
    {
        public NowPlayingView()
        {
            InitializeComponent();
            //IWaveformPlayer waveformPlayer = (IWaveformPlayer)App.AppHost!.Services.GetRequiredService<IAudioPlayer>();
            //waveformTimeline.RegisterSoundPlayer(waveformPlayer);
        }
    }
}
