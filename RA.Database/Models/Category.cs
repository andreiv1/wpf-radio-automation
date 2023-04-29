using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    [Table("Categories")]
    public partial class Category : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public int? ParentId { get; set; }

        public Category CategoryParent { get; set; }

        public string Color { get; set; }

        public ICollection<Category> Subcategories { get; set; }
        public ICollection<Track> Tracks { get; set; }
    }
}
