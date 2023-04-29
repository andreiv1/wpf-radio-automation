using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class TagValueDto
    {
        public int Id { get; private set; }
        public string? Name { get; set; }
        public int TagCategoryId { get; set; }

        public static TagValueDto FromEntity(TagValue entity)
        {
            return new TagValueDto { 
                Id = entity.Id, 
                Name = entity.Name, 
                TagCategoryId = entity.TagCategoryId 
            };
        }
    }
}
