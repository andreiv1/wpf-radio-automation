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
    public class TrackHistoryConfig : IEntityTypeConfiguration<TrackHistory>
    {
        public void Configure(EntityTypeBuilder<TrackHistory> builder)
        {
            builder.Property(th => th.TrackType)
                .HasColumnType("tinyint(4)")
                .IsRequired();

            builder.Property(th => th.Artists)
                .HasMaxLength(1000);

            builder.Property(th => th.Title)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(t => t.ISRC).HasMaxLength(55);

            builder.Property(th => th.Category)
                .HasMaxLength(100);


        }
    }
}
