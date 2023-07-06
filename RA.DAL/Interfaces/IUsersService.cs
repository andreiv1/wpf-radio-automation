using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL.Interfaces
{
    public interface IUsersService
    {
        Task<bool> AddUser(UserDTO userDTO);
        Task<UserDTO?> LogIn(string username, string password);
    }
}
