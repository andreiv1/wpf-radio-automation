using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Config
{
    public class TagCategoryConfig : IEntityTypeConfiguration<TagCategory>
    {
        public void Configure(EntityTypeBuilder<TagCategory> builder)
        {
            builder.HasMany(tc => tc.Values)
                .WithOne(t => t.TagCategory)
                .HasForeignKey(t => t.TagCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
