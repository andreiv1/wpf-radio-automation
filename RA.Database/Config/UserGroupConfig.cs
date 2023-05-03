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
    public class UserGroupConfig : IEntityTypeConfiguration<UserGroup>
    {
        public void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            builder.HasMany(ur => ur.Rules)
                .WithMany(ug => ug.Groups)
                .UsingEntity<Dictionary<string, object>>(
                "UserGroups_UserRules",
                j => j
                    .HasOne<UserRule>()
                    .WithMany()
                    .HasForeignKey("UserRuleId")
                    .HasConstraintName("FK_UserGroupsUserRules_UserRuleId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<UserGroup>()
                    .WithMany()
                    .HasForeignKey("UserGroupId")
                    .HasConstraintName("FK_UserGroupsUserRules_UserGroupId")
                    .OnDelete(DeleteBehavior.Cascade)
                );


        }
    }
}
