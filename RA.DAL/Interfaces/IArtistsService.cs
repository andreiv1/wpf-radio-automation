using RA.DTO;

namespace RA.DAL
{
    public interface IArtistsService
    {
        Task<IEnumerable<ArtistDto>> GetArtistsAsync(int skip, int take);
        Task<int> GetArtistsCountAsync();
    }
}