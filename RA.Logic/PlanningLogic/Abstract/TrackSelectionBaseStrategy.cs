using RA.DAL;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.PlanningLogic.Abstract
{
    public abstract class TrackSelectionBaseStrategy
    {
        protected readonly IPlaylistsService playlistsService;
        protected readonly ITracksService tracksService;

        /// <summary>
        /// Minimum time in minutes that must pass before a track with the same artist can be selected again 
        /// </summary>
        protected int artistSeparation;

        /// <summary>
        /// Minimum time in minutes that must pass before the same track can be selected again
        /// </summary>
        protected int trackSeparation;

        /// <summary>
        /// Minimum time in minutes that must pass before a track with the same title can be selected again
        /// </summary>
        protected int titleSeparation;

        public TrackSelectionBaseStrategy(IPlaylistsService playlistsService,
                                          ITracksService tracksService,
                                          int artistSeparation,
                                          int trackSeparation,
                                          int titleSeparation)
        {
            this.playlistsService = playlistsService;
            this.tracksService = tracksService;

            this.artistSeparation = artistSeparation;
            this.trackSeparation = trackSeparation;
            this.titleSeparation = titleSeparation;
        }

        public abstract PlaylistItemTrackDTO SelectTrack(PlaylistDTO currentPlaylist);
    }
}
