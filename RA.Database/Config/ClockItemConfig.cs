using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RA.Database.Models;
using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Config
{
    public class ClockItemConfig : IEntityTypeConfiguration<ClockItemBase>
    {
        public void Configure(EntityTypeBuilder<ClockItemBase> builder)
        {
            builder.HasDiscriminator<int>("ClockItemType")
                .HasValue<ClockItemTrack>(0)
                .HasValue<ClockItemCategory>(1)
                .HasValue<ClockItemEvent>(2);

            builder.HasOne(ci => ci.ClockItemEvent)
                .WithMany(cie => cie.EventSubitems)
                .HasForeignKey(ci => ci.ClockItemEventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
