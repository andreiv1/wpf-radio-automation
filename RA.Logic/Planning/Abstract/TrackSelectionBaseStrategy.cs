using Microsoft.EntityFrameworkCore;
using RA.Database;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.Planning.Abstract
{
    public class TrackSelectionOptions
    {
        public int? ArtistSeparation { get; set; }
        public int? TitleSeparation { get; set; }
        public int? TrackSeparation { get; set; }
        public TimeSpan? MinDuration { get; set; }
        public TimeSpan? MaxDuration { get; set; }
        public DateTime? MinReleaseDate { get; set; }
        public DateTime? MaxReleaseDate { get; set; }
        public List<int>? TagValuesIds { get; set; }

    }
    public abstract class TrackSelectionBaseStrategy
    {
        protected readonly IDbContextFactory<AppDbContext> dbContextFactory;
        protected readonly TrackSelectionOptions options;

        protected TrackSelectionBaseStrategy(IDbContextFactory<AppDbContext> dbContextFactory, TrackSelectionOptions options)
        {
            this.dbContextFactory = dbContextFactory;
            this.options = options;
        }

        public abstract PlaylistItemDTO SelectTrack(PlaylistDTO currentPlaylist);
    }
}
