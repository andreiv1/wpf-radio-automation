using RA.DTO;

namespace RA.Logic.Planning
{
    public interface IPlaylistGenerator
    {
        PlaylistDTO GeneratePlaylistForDate(DateTime date);
    }
}
