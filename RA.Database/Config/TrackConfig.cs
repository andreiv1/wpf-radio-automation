using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RA.Database.Models;
using RA.Database.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Config
{
    public class TrackConfig : IEntityTypeConfiguration<Track>
    {
        public void Configure(EntityTypeBuilder<Track> builder)
        {
            builder.HasIndex(t => t.FilePath);
            builder.HasMany(t => t.Categories)
                .WithMany(c => c.Tracks)
                .UsingEntity<Dictionary<string, object>>(
                "Categories_Tracks",
                j => j
                    .HasOne<Category>()
                    .WithMany()
                    .HasForeignKey("CategoryId")
                    .HasConstraintName("FK_TrackCategories_CategoryId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Track>()
                    .WithMany()
                    .HasForeignKey("TrackId")
                    .HasConstraintName("FK_TrackCategories_TrackId")
                    .OnDelete(DeleteBehavior.Cascade)
                    );

            builder.HasMany(t => t.TrackTags)
                .WithOne(tt => tt.Track)
                .HasForeignKey(tt => tt.TrackId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
