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
    public class ScheduleConfig : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.HasDiscriminator<string>("Discriminator_Type")
                .HasValue<DefaultSchedule>("d")
                .HasValue<PlannedSchedule>("p");

            builder.Property("Discriminator_Type")
                .HasColumnType("char");
        }
    }
}
