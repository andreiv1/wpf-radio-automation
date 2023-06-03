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
    public class ClockItemCategoryTagConfig : IEntityTypeConfiguration<ClockItemCategoryTag>
    {
        public void Configure(EntityTypeBuilder<ClockItemCategoryTag> builder)
        {
            builder.HasKey(itmCategoryTag => 
                new { itmCategoryTag.ClockItemCategoryId, itmCategoryTag.TagValueId });

            builder.HasOne(itmCategoryTag => itmCategoryTag.ClockItemCategory)
                .WithMany(clockItemCategory => clockItemCategory.ClockItemCategoryTags)
                .HasForeignKey(itmCategoryTag => itmCategoryTag.ClockItemCategoryId);

            builder.HasOne(itmCategoryTag => itmCategoryTag.TagValue)
                .WithMany(clockItemCategory => clockItemCategory.ClockItemsCategoryTags)
                .HasForeignKey(itmCategoryTag => itmCategoryTag.TagValueId);


        }
    }
}
