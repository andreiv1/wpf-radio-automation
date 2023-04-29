using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Dto
{
    public class TrackArtistDto
    {
        public int ArtistId { get; set; }

        public string ArtistName { get; set; }

        public int TrackId { get; set; }

        public int OrderIndex { get; set; }

        public static TrackArtistDto FromEntity(ArtistTrack artistTrack)
        {
            return new TrackArtistDto { 
                ArtistId = artistTrack.ArtistId,
                ArtistName = artistTrack.Artist.Name, 
                TrackId = artistTrack.TrackId,
                OrderIndex = artistTrack.OrderIndex,
            };
        }

        public static ArtistTrack ToEntity(TrackArtistDto dto)
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
