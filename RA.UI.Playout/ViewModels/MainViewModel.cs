using RA.UI.Core.ViewModels;
using RA.UI.Playout.ViewModels.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Playout.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        public NowPlayingViewModel NowPlayingVm { get; private set; }
        public PlaylistViewModel PlaylistVm { get; private set; }
        public MediaItemsViewModel MediaItemsVm { get; private set; }

        public MainViewModel(NowPlayingViewModel nowPlayingVm,
                             PlaylistViewModel playlistVm,
                             MediaItemsViewModel mediaItemsVm)
        {
            NowPlayingVm = nowPlayingVm;
            PlaylistVm = playlistVm;
            MediaItemsVm = mediaItemsVm;
            mediaItemsVm.MainVm = this;
            playlistVm.MainVm = this;
        }

       
    }
}
