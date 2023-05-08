using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class PlaylistListingDTO
    {
        public int Id { get; set; }
        public DateTime AirDate { get; set; }

        public static PlaylistListingDTO FromEntity(Playlist entity)
        {
            return new PlaylistListingDTO
            {
                Id = entity.Id,
                AirDate = entity.AirDate
            };
        }
    }
}
