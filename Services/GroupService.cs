using System.Text;
using Attendiia.Models;
using Attendiia.Services.Interface;

namespace Attendiia.Services;

public sealed class GroupService : IGroupService
{
    private readonly IFirebaseDatabaseService _firebaseaDatabaseService;
    public GroupService(IFirebaseDatabaseService firebaseDatabaseService)
    {
        _firebaseaDatabaseService = firebaseDatabaseService;
    }

    public async Task<string> CreateGroupAsync(string groupName)
    {
        if (string.IsNullOrEmpty(groupName))
        {
            throw new ArgumentException($"{nameof(groupName)} is null or empty.");
        }
        string code = GenerateCode();
        while (await IsExistsAsync(code))
        {
            code = GenerateCode();
        }
        await _firebaseaDatabaseService.AddItemAsync(FirebaseDatabaseKeys.GROUP_PATH, code, new Group(code, groupName));
        return code;
    }

    public async Task DeleteGroupAsync(string groupCode)
    {
        if (string.IsNullOrEmpty(groupCode))
        {
            throw new ArgumentException($"{nameof(groupCode)} is null or empty.");
        }
        if (!await IsExistsAsync(groupCode))
        {
            throw new ArgumentException($"{nameof(groupCode)} is not found.");
        }
        await _firebaseaDatabaseService.DeleteItemAsync(FirebaseDatabaseKeys.GROUP_PATH, groupCode);
    }

    public string GenerateCode()
    {
        StringBuilder sb = new StringBuilder(6);
        for (int i = 0; i < sb.Capacity; i++)
        {
            sb.Append(Convert.ToChar(Random.Shared.Next(65, 65 + 26)));
        }
        return sb.ToString();
    }

    public async Task<Group?> GetGroupByCodeAsync(string groupCode)
    {
        return await _firebaseaDatabaseService.GetItemAsync<Group?>(FirebaseDatabaseKeys.GROUP_PATH, groupCode);
    }

    public async Task<List<Group>> GetGroupsAsync()
    {
        return await _firebaseaDatabaseService.GetItemsAsync<Group>(FirebaseDatabaseKeys.GROUP_PATH);
    }

    public async Task<List<Group>> GetGroupsByEmailAsync(string email)
    {
        List<Group> groups = new List<Group>();
        List<GroupUser> groupUsers = await _firebaseaDatabaseService.GetItemsAsync<GroupUser>(FirebaseDatabaseKeys.GROUP_USER_PATH);
        foreach (var groupUser in groupUsers)
        {
            Group? group = await _firebaseaDatabaseService.GetItemAsync<Group>(FirebaseDatabaseKeys.GROUP_PATH, groupUser.GroupCode);
            if (group == null)
            {
                throw new ArgumentNullException($"{nameof(group)} is null.");
            }
            groups.Add(group);
        }
        return groups;
    }

    public async Task UpdateGroupAsync(string groupCode, Group group)
    {
        if (!await IsExistsAsync(groupCode))
        {
            throw new ArgumentException($"{nameof(groupCode)} is not found.");
        }
        await _firebaseaDatabaseService.UpdateItemAsync(FirebaseDatabaseKeys.GROUP_PATH, groupCode, group);
    }

    private async Task<bool> IsExistsAsync(string code)
    {
        Group? group = await GetGroupByCodeAsync(code);
        return group != null;
    }
}
