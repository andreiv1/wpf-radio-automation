using RA.DAL;
using RA.DTO;
using RA.Logic.PlanningLogic.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.PlanningLogic
{
    public class RandomTrackSelectionStrategy : TrackSelectionBaseStrategy
    {
        public RandomTrackSelectionStrategy(IPlaylistsService playlistsService, ITracksService tracksService, 
            int artistSeparation = 30, int trackSeparation = 60, int titleSeparation = 30) 
            : base(playlistsService, tracksService, artistSeparation, trackSeparation, titleSeparation)
        {
        }

        public override PlaylistItemTrackDTO SelectTrack(PlaylistDTO playlist)
        {
            //TODO: check from previous playlists for repetition
            throw new NotImplementedException();
        }
    }
}
