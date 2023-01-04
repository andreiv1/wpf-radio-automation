using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class CategoryTreeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }

        public static CategoryTreeDTO FromEntity(Category category)
        {
            return new CategoryTreeDTO
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentId,
            };
        }
    }
}
