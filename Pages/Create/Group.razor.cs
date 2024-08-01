using System.Security.Claims;
using Attendiia.Forms;
using Attendiia.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Attendiia.Pages.Create;
public partial class Group
{
    [CascadingParameter]
    public Task<AuthenticationState> AuthenticationState { get; set; } = null!;
    private GroupCreateForm groupCreateForm = new GroupCreateForm();
    private bool isLoading = false;
    private string message = string.Empty;

    private async Task OnValidRequest()
    {
        isLoading = true;
        try
        {
            string groupCode = await GroupService.CreateGroupAsync(groupCreateForm.Name);
            var authenticationState = await AuthenticationState;
            string? uid = authenticationState.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;
            if (uid == default)
            {
                throw new Exception("ユーザ情報の取得に失敗しました。");
            }
            Models.Group group = await GroupService.GetGroupByCodeAsync(groupCode);
            List<GroupUser> groupUsers = await GroupUserService.GetGroupUsersByUidAsync(uid);
            GroupUser? existsGroupUser = null;
            foreach (GroupUser groupUser in groupUsers)
            {
                groupUser.IsCurrent = false;
                if (groupUser.GroupCode == group.GroupCode)
                {
                    existsGroupUser = groupUser;
                    existsGroupUser.IsCurrent = true;
                }
                await GroupUserService.UpdateGroupAsync(groupUser.Id, groupUser);
            }
            if (existsGroupUser == null)
            {
                GroupUser groupUser = new GroupUser(
                        Guid.NewGuid().ToString(),
                        group.GroupCode,
                        uid)
                {
                    IsCurrent = true
                };
                await GroupUserService.CreateGroupUserAsync(groupUser);
            }
            else
            {
                await GroupUserService.UpdateGroupAsync(existsGroupUser.Id, existsGroupUser);
            }
            NavigationManager.NavigateTo("", true);
        }
        catch (NotSupportedException e)
        {
            message = e.Message;
        }
        catch (Exception e)
        {
            message = e.Message;
        }
        finally
        {
            isLoading = false;
        }
    }
}
