using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DTO
{
    public class UserDTO
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }

        public string GroupName { get; set; }
        public int GroupId { get; set; }

        public List<UserRuleType> GroupRules { get; set; }

        public static UserDTO FromEntity(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                GroupName = user.UserGroup.Name,
                GroupId = user.UserGroup.Id,
                GroupRules = user.UserGroup.Rules.Select(r => r.RuleValue).ToList()
            };
        }

        public static User ToEntity(UserDTO dto)
        {
            return new User
            {
                Id = dto.Id ?? 0,
                Username = dto.Username,
                Password = dto.Password,
                FullName = dto.FullName,
                UserGroup = new UserGroup
                {
                    Id = dto.GroupId,
                }
            };
        }
    }
}
