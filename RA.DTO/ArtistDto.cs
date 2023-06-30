using RA.Database.Models;

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

        public static Artist ToEntity(ArtistDTO dto)
        {
            return new Artist { Id = dto.Id, Name = dto.Name };
        }
    }
}
