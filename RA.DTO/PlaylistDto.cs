using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Dto
{
    public class PlaylistDto
    {
        public int Id { get; set; }

        public DateTime AirDate { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateModified { get; set; }

        public static PlaylistDto FromEntity(Playlist entity)
        {
            return new PlaylistDto { Id = entity.Id, AirDate = entity.AirDate, DateAdded = entity.DateAdded, DateModified = entity.DateModified };
        }
    }
}
