using Attendiia.Models;

namespace Attendiia.Services.Interface;

public interface IGroupUserService
{
    Task<string> CreateGroupUserAsync(GroupUser groupUser);
    Task<List<GroupUser>> GetGroupUsersAsync();
    Task<List<GroupUser>> GetGroupUsersByUidAsync(string uid);
    Task<GroupUser> GetGroupUserByIdAsync(string id);
    Task<GroupUser?> GetGroupUserIsCurrentAsync(string uid);
    Task UpdateGroupAsync(string id, GroupUser groupUser);
    Task DeleteGroupAsync(string id);
}
