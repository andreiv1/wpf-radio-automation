using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.AudioPlayer.Interfaces
{
    public interface IPlayerItem
    {
        public string FilePath { get; }
        public TimeSpan Duration { get; }
        public DateTime ETA { get; set; }
        public string? Artists { get; }
        public string Title { get; }
    }
}
