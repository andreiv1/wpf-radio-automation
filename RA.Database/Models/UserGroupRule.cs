using RA.Database.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database.Models
{
    public enum UserRuleType
    {
        ACCESS_MEDIA_LIBRARY,
        ACCESS_PLANNER,
        ACCESS_REPORTS,
        ACCESS_SETTINGS,
    }
    public class UserGroupRule : BaseModel
    {
        [Required]
        [EnumDataType(typeof(UserRuleType))]
        public UserRuleType RuleValue { get; set; }

        [Required]
        public UserGroup UserGroup { get; set; }
    }
}
