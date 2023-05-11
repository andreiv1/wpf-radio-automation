using RA.Logic.AudioPlayer.Interfaces;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Playout.ViewModels.Components
{
    public partial class PlaylistViewModel : ViewModelBase
    {
        private IPlaybackQueue playbackQueue;

        private IPlayerItem? playerItemNow;
        public MainViewModel MainVm { get; set; }

        #region Constructor
        public PlaylistViewModel(IPlaybackQueue playbackQueue)
        {
            this.playbackQueue = playbackQueue;
        }

        #endregion
    }
}
