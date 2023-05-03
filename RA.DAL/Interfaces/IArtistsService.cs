using RA.DTO;

namespace RA.DAL
{
    public interface IArtistsService
    {
        Task<IEnumerable<ArtistDTO>> GetArtistsAsync(int skip, int take);
        Task<int> GetArtistsCountAsync();
    }
}