using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    /// <summary>
    /// Data model used for CategoriesHierarchy View
    /// </summary>
    [NotMapped]
    public class CategoryHierarchy
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int? ParentId { get; set; }
        public String PathName { get; set; }
        public int Level { get; set; }

        public override string ToString()
        {
            return $"Id={Id},ParentId={ParentId},PathName={PathName},Level={Level}";
        }
    }
}
