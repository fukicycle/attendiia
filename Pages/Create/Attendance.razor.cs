using System.Security.Claims;
using Attendiia.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Attendiia.Pages.Create;

public partial class Attendance
{
    private AttendanceCreateForm attendanceCreateForm = new AttendanceCreateForm();
    private bool isLoading = false;

    [CascadingParameter]
    private Task<AuthenticationState>? authenticationState { get; set; }

    protected override async Task OnInitializedAsync()
    {
        isLoading = true;
        if (authenticationState == null)
        {
            throw new ArgumentNullException("ログインユーザの情報を取得する際にエラーが発生しました。管理者へ連絡してください。");
        }
        AuthenticationState state = await authenticationState;
        string? displayName = state.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Name)?.Value;
        string? groupCode = state.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.UserData)?.Value;
        if (string.IsNullOrEmpty(displayName) || string.IsNullOrEmpty(groupCode))
        {
            throw new ArgumentNullException("ログインに成功していますが、認証情報を取得することができません。管理者へ連絡してください。");
        }
        attendanceCreateForm.AuthorDisplayName = displayName;
        attendanceCreateForm.GroupCode = groupCode;
        await base.OnInitializedAsync();
        isLoading = false;
    }

    private async Task OnValidRequest()
    {
        isLoading = true;
        await AttendanceService.CreateAttendanceAsync(attendanceCreateForm);
        NavigationManager.NavigateTo("");
    }
}
