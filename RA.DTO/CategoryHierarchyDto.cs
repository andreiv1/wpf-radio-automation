using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class CategoryHierarchyDTO
    {
        public int Id { get; set; }
        public string PathName { get; set; }

        public int? ParentId { get; set; }

        public int Level { get; set; }

        public static CategoryHierarchyDTO FromEntity(CategoryHierarchy categoryHierarchy)
        {
            return new CategoryHierarchyDTO { 
                Id = categoryHierarchy.Id, 
                PathName = categoryHierarchy.PathName, 
                ParentId = categoryHierarchy.ParentId,
                Level = categoryHierarchy.Level,
            };
        }
    }
}
