using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class CategoryTreeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }

        public static CategoryTreeDto FromEntity(Category category)
        {
            return new CategoryTreeDto
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentId,
            };
        }
    }
}
