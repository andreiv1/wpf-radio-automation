using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Dto
{
    public class CategoryDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }

        public string Color { get; set; }

        public static CategoryDto FromEntity(Category category)
        {
            return new CategoryDto { 
                Id = category.Id, 
                Name = category.Name, 
                Description = category.Description, 
                ParentId = category.ParentId, 
                Color = category.Color
            };
        }

        public static Category FromDto(CategoryDto dto)
        {
            return new Category
            {
                Id = dto.Id ?? 0,
                Name = dto.Name,
                Description = dto.Description,
                ParentId = dto.ParentId,
                Color = dto.Color
            };
        }
    }
}
