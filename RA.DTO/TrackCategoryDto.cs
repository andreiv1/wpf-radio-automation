using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class TrackCategoryDTO
    {
        public int CategoryId { get; set; }
        public String CategoryName { get; set; }

        public static TrackCategoryDTO FromEntity(Category category)
        {
            return new TrackCategoryDTO()
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
            };
        }

        public static Category ToEntity(TrackCategoryDTO dto)
        {
            return new Category()
            {
                Id = dto.CategoryId
            };
        }
    }
}
