using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class UserGroupDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public bool IsBuiltin { get; set; }
        public List<UserRuleType>? Rules { get; set; }

        public static UserGroup ToEntity(UserGroupDTO dto)
        {
            ICollection<UserGroupRule> rules = dto.Rules.Select(r => new UserGroupRule
            {
                RuleValue = r
            }).ToList();
            return new UserGroup
            {
                Id = dto.Id.GetValueOrDefault(),
                Name = dto.Name,
                Rules = rules,
            };
        }

        public static UserGroupDTO FromEntity(UserGroup entity)
        {
            return new UserGroupDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Rules = entity.Rules != null ? entity.Rules.Select(r => r.RuleValue).ToList() : null,
                IsBuiltin = entity.IsBuiltIn,
            };
        }
    }
}
