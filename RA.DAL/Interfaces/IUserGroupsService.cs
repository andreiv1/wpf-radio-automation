using RA.DTO;

namespace RA.DAL.Interfaces
{
    public interface IUserGroupsService
    {
        Task AddGroup(UserGroupDTO userGroupDTO);
        Task<bool> CheckExistingGroupByName(string name);
        Task<UserGroupDTO?> GetGroup(int id);
        Task<ICollection<UserGroupDTO>> GetGroups();
        Task<ICollection<UserDTO>> GetUsersByGroup(int groupId);
        Task UpdateGroup(UserGroupDTO userGroupDTO);
    }
}