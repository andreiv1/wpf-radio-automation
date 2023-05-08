using RA.Database.Models;
using RA.DTO.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class PlaylistDTO
    {
        public int Id { get; set; }

        public DateTime AirDate { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateModified { get; set; }

        public List<PlaylistItemBaseDTO>? Items { get; set; }

        public static PlaylistDTO FromEntity(Playlist entity)
        {
            //TODO: get items
            return new PlaylistDTO { 
                Id = entity.Id, 
                AirDate = entity.AirDate, 
                DateAdded = entity.DateAdded, 
                DateModified = entity.DateModified 
            };
        }

        public static Playlist ToEntity(PlaylistDTO dto)
        {
            //TODO: add items
            return new Playlist
            {
                Id = dto.Id,
                AirDate = dto.AirDate,
                DateAdded = dto.DateAdded,

            };
        }
    }
}
