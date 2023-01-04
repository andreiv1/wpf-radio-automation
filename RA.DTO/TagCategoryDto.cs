using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class TagCategoryDTO
    {
        public int? Id { get; private set; }
        public string Name { get; set; } = "";
        public static TagCategoryDTO FromEntity(TagCategory entity)
        {
            return new TagCategoryDTO
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }
    }
}
