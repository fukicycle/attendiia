using Attendiia.Models;

namespace Attendiia.Services.Interface;

public interface IGroupUserService
{
    Task<string> CreateGroupUserAsync(GroupUser groupUser);
    Task<List<GroupUser>> GetGroupUsersAsync();
    Task<List<GroupUser>> GetGroupUsersByEmailAsync(string email);
    Task<GroupUser> GetGroupUserByIdAsync(string id);
    Task<GroupUser?> GetGroupUserIsCurrentAsync(string email);
    Task UpdateGroupAsync(string id, GroupUser groupUser);
    Task DeleteGroupAsync(string id);
}
