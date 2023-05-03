﻿using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class CategoryDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }

        public string Color { get; set; }

        public static CategoryDTO FromEntity(Category category)
        {
            return new CategoryDTO { 
                Id = category.Id, 
                Name = category.Name, 
                Description = category.Description, 
                ParentId = category.ParentId, 
                Color = category.Color
            };
        }

        public static Category FromDto(CategoryDTO dto)
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
