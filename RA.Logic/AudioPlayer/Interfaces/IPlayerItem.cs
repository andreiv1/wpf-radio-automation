namespace RA.Logic.AudioPlayer.Interfaces
{
    public interface IPlayerItem
    {
        public int TrackId { get; }
        public string FilePath { get; }
        public string ImagePath { get; }
        public TimeSpan Duration { get; }
        public DateTime ETA { get; set; }
        public string? Artists { get; }
        public string Title { get; }
        public string? TrackType { get; }
    }
}
