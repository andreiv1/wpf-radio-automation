using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Dto
{
    public class TagCategoryDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public static TagCategoryDto FromEntity(TagCategory entity)
        {
            return new TagCategoryDto
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }
    }
}
