using Attendiia.Models;
using Attendiia.Services.Interface;

namespace Attendiia.Services;

public sealed class GroupUserService : IGroupUserService
{
    private readonly IFirebaseDatabaseService _firebaseDatabaseService;
    private readonly ILogger<GroupUserService> _logger;
    public GroupUserService(IFirebaseDatabaseService firebaseDatabaseService, ILogger<GroupUserService> logger)
    {
        _firebaseDatabaseService = firebaseDatabaseService;
        _logger = logger;
    }
    public async Task<string> CreateGroupUserAsync(GroupUser groupUser)
    {
        if (await IsExistsAsync(groupUser))
        {
            throw new ArgumentException($"{nameof(groupUser)} is already added.");
        }
        await _firebaseDatabaseService.AddItemAsync(
            FirebaseDatabaseKeys.GROUP_USER_PATH, groupUser.Id, groupUser);
        return groupUser.Id;
    }

    public async Task DeleteGroupAsync(string id)
    {
        await GetGroupUserByIdAsync(id);
        await _firebaseDatabaseService.DeleteItemAsync(
            FirebaseDatabaseKeys.GROUP_USER_PATH, id);
    }

    public async Task<GroupUser> GetGroupUserByIdAsync(string id)
    {
        return await _firebaseDatabaseService.GetItemAsync<GroupUser>(
            FirebaseDatabaseKeys.GROUP_USER_PATH, id);
    }

    public async Task<GroupUser?> GetGroupUserIsCurrentAsync(string email)
    {
        List<GroupUser> groupUsers = await GetGroupUsersByEmailAsync(email);
        return groupUsers.SingleOrDefault(a => a.IsCurrent);
    }

    public async Task<List<GroupUser>> GetGroupUsersAsync()
    {
        return await _firebaseDatabaseService.GetItemsAsync<GroupUser>(
            FirebaseDatabaseKeys.GROUP_USER_PATH);
    }

    public async Task<List<GroupUser>> GetGroupUsersByEmailAsync(string email)
    {
        List<GroupUser> groupUsers = await GetGroupUsersAsync();
        return groupUsers.Where(a => a.Email == email).ToList();
    }

    public async Task UpdateGroupAsync(string id, GroupUser groupUser)
    {
        await GetGroupUserByIdAsync(id);
        await _firebaseDatabaseService.UpdateItemAsync(
            FirebaseDatabaseKeys.GROUP_USER_PATH, id, groupUser);
    }

    private async Task<bool> IsExistsAsync(GroupUser groupUser)
    {
        List<GroupUser> groupUsers = await GetGroupUsersAsync();
        return groupUsers.Any(a => a.Email == groupUser.Email && a.GroupCode == groupUser.GroupCode);
    }
}
