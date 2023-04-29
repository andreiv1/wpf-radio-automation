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
    public class TrackTagConfig : IEntityTypeConfiguration<TrackTag>
    {
        public void Configure(EntityTypeBuilder<TrackTag> builder)
        {
            builder.HasKey(tt => new { tt.TrackId, tt.TagValueId });

            builder.HasOne(tt => tt.Track)
                .WithMany(t => t.TrackTags)
                .HasForeignKey(tt => tt.TrackId);

            builder.HasOne(tt => tt.TagValue)
               .WithMany(t => t.TrackTags)
               .HasForeignKey(tt => tt.TagValueId);
        }
    }
}
