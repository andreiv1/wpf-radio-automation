using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    public class ClockItemCategory : ClockItemBase
    {
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public TimeSpan? MinDuration { get; set; }
        public TimeSpan? MaxDuration { get; set; }
        public int? ArtistSeparation { get; set; }
        public int? TitleSeparation { get; set; }
        public int? TrackSeparation { get; set; }
        public DateTime? MinReleaseDate { get; set; }
        public DateTime? MaxReleaseDate { get; set; }
        public Boolean IsFiller { get; set; } = false;
        public ICollection<ClockItemCategoryTag> ClockItemCategoryTags { get; set; }


    }
}
