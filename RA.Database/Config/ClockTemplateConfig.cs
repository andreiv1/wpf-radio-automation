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
    public class ClockTemplateConfig : IEntityTypeConfiguration<ClockTemplate>
    {
        public void Configure(EntityTypeBuilder<ClockTemplate> builder)
        {
            builder.HasKey(ct => new { ct.ClockId, ct.TemplateId, ct.StartTime});

            builder.HasOne(ct => ct.Clock)
               .WithMany(c => c.ClockTemplates)
               .HasForeignKey(ct => ct.ClockId);

            builder.HasOne(ct => ct.Template)
                .WithMany(c => c.TemplateClocks)
                .HasForeignKey(ct => ct.TemplateId);
        }
    }
}
