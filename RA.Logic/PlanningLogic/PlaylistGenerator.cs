using RA.DAL;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.PlanningLogic
{
    public class PlaylistGenerator : IPlaylistGenerator
    {
        private readonly IPlaylistsService playlistsService;
        private readonly IClocksService clocksService;

        public PlaylistGenerator(IPlaylistsService playlistsService, IClocksService clocksService)
        {
            this.playlistsService = playlistsService;
            this.clocksService = clocksService;
        }

        public PlaylistDTO GeneratePlaylist(DateTime date)
        {
            var result = InitialisePlaylist(date);
            DateTime estimatedPlaylistStart = new DateTime(date.Year, date.Month, date.Day);
            
            return result;
        }

        private PlaylistDTO InitialisePlaylist(DateTime date)
        {
            var result = new PlaylistDTO();
            result.AirDate = date;
            result.DateAdded = DateTime.Now;
            result.Items = new List<PlaylistItemDTO>();

            return result;
        }
    }
}
