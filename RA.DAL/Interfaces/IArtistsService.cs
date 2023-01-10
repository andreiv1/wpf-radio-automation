using RA.DTO;

namespace RA.DAL
{
    public interface IArtistsService
    {
        Task AddArtist(ArtistDTO artist);
        Task<ArtistDTO?> GetArtistByName(string name);
        Task<IEnumerable<ArtistDTO>> GetArtistsAsync(int skip, int take, string query = "");
        Task<int> GetArtistsCountAsync(string query = "");
    }
}