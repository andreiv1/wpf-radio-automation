using RA.UI.Core.ViewModels;
using RA.UI.Playout.ViewModels.Components;

namespace RA.UI.Playout.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        public NowPlayingViewModel NowPlayingVm { get; private set; }
        public PlaylistViewModel PlaylistVm { get; private set; }
        public MediaItemsViewModel MediaItemsVm { get; private set; }
        public HistoryViewModel HistoryVm { get; private set; }

        public MainViewModel(NowPlayingViewModel nowPlayingVm,
                             PlaylistViewModel playlistVm,
                             MediaItemsViewModel mediaItemsVm,
                             HistoryViewModel historyVm)
        {
            NowPlayingVm = nowPlayingVm;
            PlaylistVm = playlistVm;
            MediaItemsVm = mediaItemsVm;
            HistoryVm = historyVm;
            mediaItemsVm.MainVm = this;
            playlistVm.MainVm = this;
            historyVm.MainVm = this;
            nowPlayingVm.PlaylistVm = playlistVm;
            
        }

       
    }
}
