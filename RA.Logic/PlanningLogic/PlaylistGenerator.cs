using RA.DAL;
using RA.DTO;
using RA.DTO.Abstract;
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
        private readonly ITemplatesService templatesService;

        public PlaylistGenerator(IPlaylistsService playlistsService, IClocksService clocksService,
            ITemplatesService templatesService)
        {
            this.playlistsService = playlistsService;
            this.clocksService = clocksService;
            this.templatesService = templatesService;
        }

        public PlaylistDTO GeneratePlaylistForDate(DateTime date)
        {
            var result = InitialisePlaylist(date);
            DateTime estimatedPlaylistStart = new DateTime(date.Year, date.Month, date.Day);
            
            return result;
        }

        private PlaylistDTO InitialisePlaylist(DateTime date)
        {
            var playlist = new PlaylistDTO();
            playlist.AirDate = date;
            playlist.DateAdded = DateTime.Now;
            playlist.Items = new List<PlaylistItemBaseDTO>();

            return playlist;
        }
    }
}
