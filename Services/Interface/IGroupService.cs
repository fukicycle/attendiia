using Attendiia.Models;

namespace Attendiia.Services.Interface;

public interface IGroupService
{
    Task<string> CreateGroupAsync(string groupName);
    Task<Group> GetGroupByCodeAsync(string groupCode);
    Task UpdateGroupAsync(string groupCode, Group group);
    Task DeleteGroupAsync(string groupCode);
}
