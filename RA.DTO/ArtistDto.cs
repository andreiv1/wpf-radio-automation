using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Dto
{
    public class ArtistDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public static ArtistDto FromEntity(Artist artist)
        {
            return new ArtistDto { Id = artist.Id, Name = artist.Name };
        }
    }
}
