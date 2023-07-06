using RA.Database.Models;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.StationManagement.Stores
{
    public class UserStore
    {
        public UserDTO? LoggedUser { get; set; }

        public bool SessionLocked { get; set; }

        public bool CheckPermissions(UserRuleType rule)
        {
            //return LoggedUser?.GroupRules.Any(r => r == rule) ?? false;
            return true;
        }
    }
}
