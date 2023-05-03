using RA.DTO;
using RA.Logic.TrackFileLogic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.TrackFileLogic.Models
{
    public class ProcessingTrack
    {
        public TrackDTO TrackDto { get; set; }
        public ProcessingTrackStatus Status { get; set; }

        public string? CategoryName => TrackDto.Categories != null && TrackDto.Categories.Count > 0
            ? TrackDto.Categories.ElementAt(0).CategoryName : null;

        public string? Artists => TrackDto.Artists != null
            ? string.Join(", ", TrackDto.Artists.Select(a => a.ArtistName)) : null;
    }
}
