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
        string? email = state.User.Identity?.Name;
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentNullException("ログインに成功していますが、認証情報を取得することができません。管理者へ連絡してください。");
        }
        attendanceCreateForm.AuthorEmail = email;
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
