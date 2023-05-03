using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class ArtistDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public static ArtistDTO FromEntity(Artist artist)
        {
            return new ArtistDTO { Id = artist.Id, Name = artist.Name };
        }
    }
}
