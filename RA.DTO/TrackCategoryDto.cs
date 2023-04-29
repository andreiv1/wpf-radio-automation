using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Dto
{
    public class TrackCategoryDto
    {
        public int CategoryId { get; set; }
        public String CategoryName { get; set; }

        public static TrackCategoryDto FromEntity(Category category)
        {
            return new TrackCategoryDto()
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
            };
        }

        public static Category ToEntity(TrackCategoryDto dto)
        {
            return new Category()
            {
                Id = dto.CategoryId
            };
        }
    }
}
