using Microsoft.EntityFrameworkCore;
using RA.DAL.Exceptions;
using RA.DAL.Interfaces;
using RA.Database;
using RA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL
{
    public class UserGroupsService : IUserGroupsService
    {
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;

        public UserGroupsService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task AddGroup(UserGroupDTO userGroupDTO)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var entity = UserGroupDTO.ToEntity(userGroupDTO);
            await dbContext.UserGroups.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateGroup(UserGroupDTO userGroupDTO)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            if (userGroupDTO.Id == 1 || userGroupDTO.Name == "Administrators")
            {
                throw new UnauthorizedAccessException($"This group can't be modified!");
            }
            var entity = UserGroupDTO.ToEntity(userGroupDTO);
            
            var existingEntity = await dbContext.UserGroups
                .Include(ug => ug.Rules)
                .SingleOrDefaultAsync(ug => ug.Id == userGroupDTO.Id);
            if(existingEntity != null)
            {
                dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);

                //Delete not existing rules
                foreach(var existingRule in existingEntity.Rules)
                {
                    if(!entity.Rules.Any(newRule => newRule.Id == existingRule.Id))
                    {
                        existingEntity.Rules.Remove(existingRule);
                    }
                }
                //Add new rules
                foreach (var rule in entity.Rules)
                {
                    existingEntity.Rules.Add(rule);
                }

                
                
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<ICollection<UserGroupDTO>> GetGroups()
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var result = await dbContext.UserGroups
                .Include(ug => ug.Rules)
                .Select(ug => UserGroupDTO.FromEntity(ug))
                .AsNoTracking()
                .ToListAsync();

            return result;
        }

        public async Task<ICollection<UserDTO>> GetUsersByGroup(int groupId)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var result = await dbContext.Users
                .Where(x => x.UserGroupId == groupId)
                .Select(x =>  UserDTO.FromEntity(x))
                .ToListAsync();

            return result;
        }

        public async Task<UserGroupDTO?> GetGroup(int id)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            var result = await dbContext.UserGroups
                .Include(ug => ug.Rules)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (result == null) throw new NotFoundException($"Group with {id} does not exist!");
            return UserGroupDTO.FromEntity(result);
        }

        public async Task<bool> CheckExistingGroupByName(string name)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            return await dbContext.UserGroups.Where(x => x.Name == name).AnyAsync();
        }
    }
}
