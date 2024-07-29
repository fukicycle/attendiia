using Attendiia.Models;
using Attendiia.Services.Interface;

namespace Attendiia.Services;

public sealed class GroupUserService : IGroupUserService
{
    private readonly IFirebaseDatabaseService _firebaseDatabaseService;
    public GroupUserService(IFirebaseDatabaseService firebaseDatabaseService)
    {
        _firebaseDatabaseService = firebaseDatabaseService;
    }
    public Task<string> CreateGroupUserAsync(GroupUser groupUser)
    {
        throw new NotImplementedException();
    }

    public Task DeleteGroupAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<List<GroupUser>> GetGroupUsersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<GroupUser>> GetGroupUsersByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task UpdateGroupAsync(string id, GroupUser groupUser)
    {
        throw new NotImplementedException();
    }
}
