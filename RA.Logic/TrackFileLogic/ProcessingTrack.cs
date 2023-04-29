using RA.Dto;
using RA.Logic.TrackFileLogic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.TrackFileLogic
{
    public class ProcessingTrack
    {
        public TrackDto TrackDto { get; set; }
        public ProcessingTrackStatus Status { get; set; }

        public String? CategoryName => TrackDto.Categories != null && TrackDto.Categories.Count > 0 
            ? TrackDto.Categories.ElementAt(0).CategoryName : null;

        public String? Artists => TrackDto.Artists != null
            ? String.Join(", ", TrackDto.Artists.Select(a => a.ArtistName)) : null;
    }
}
