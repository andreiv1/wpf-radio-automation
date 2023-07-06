using Microsoft.EntityFrameworkCore;
using RA.DAL.Interfaces;
using RA.DAL.Utils;
using RA.Database;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public class UsersService : IUsersService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;
        public UsersService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<bool> CanUserLogIn(string username, string password)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var user = await dbContext.Users
                .Where(u => u.Username == username)
                .FirstOrDefaultAsync();
            if (user == null) return false;

            bool isPasswordValid = PasswordEncryptor.VerifyPassword(password, user.Password);
            return isPasswordValid;
        }

        public async Task<bool> AddUser(UserDTO userDTO)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == userDTO.Username);
            if (existingUser != null) 
                return false;

            string encryptedPassword = PasswordEncryptor.EncryptPassword(userDTO.Password);
            var userEntity = UserDTO.ToEntity(userDTO);
            userEntity.UserGroup = dbContext.AttachOrGetTrackedEntity(userEntity.UserGroup);
            userEntity.Password = encryptedPassword;
            dbContext.Users.Add(userEntity);
            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
