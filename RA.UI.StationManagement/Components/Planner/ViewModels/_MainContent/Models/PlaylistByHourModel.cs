using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Components.Planner.ViewModels._MainContent.Models
{
    public class PlaylistByHourModel
    {
        public int Hour { get; set; }

        public TimeSpan HourAsTimeSpan => TimeSpan.FromHours(Hour);
        public double Length { get; set; }
        public TimeSpan LengthAsTimeSpan => TimeSpan.FromSeconds(Length);
        public int PlaylistId { get; set; }

        public static PlaylistByHourModel FromDTO(PlaylistByHourDTO dto)
        {
            return new PlaylistByHourModel
            {
                Hour = dto.Hour,
                Length = dto.Length,
                PlaylistId = dto.PlaylistId
            };
        }
    }
}
