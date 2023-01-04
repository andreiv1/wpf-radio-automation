using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class TrackArtistDTO
    {
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
        public int TrackId { get; set; }
        public int OrderIndex { get; set; }

        public static TrackArtistDTO FromEntity(ArtistTrack artistTrack)
        {
            return new TrackArtistDTO { 
                ArtistId = artistTrack.ArtistId,
                ArtistName = artistTrack.Artist.Name, 
                TrackId = artistTrack.TrackId,
                OrderIndex = artistTrack.OrderIndex,
            };
        }

        public static ArtistTrack ToEntity(TrackArtistDTO dto)
        {
            return new ArtistTrack
            {
                ArtistId = dto.ArtistId,
                TrackId = dto.TrackId,
                OrderIndex = dto.OrderIndex,
            };
        }
    }
}
