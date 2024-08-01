using Attendiia.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Attendiia.Pages;

public partial class AttendanceDetail
{
    [Parameter]
    [SupplyParameterFromQuery(Name = "id")]
    public string Id { get; set; } = string.Empty;

    [CascadingParameter]
    public Task<AuthenticationState> AuthenticationState { get; set; } = null!;
    private Attendance? attendance;
    private bool isLoading = false;
    private string message = string.Empty;
    private string uid = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var authenticationState = await AuthenticationState;
        string? uid = authenticationState.User.Identity?.Name;
        if (string.IsNullOrEmpty(uid))
        {
            message = "認証に失敗しました。";
            NavigationManager.NavigateTo("", true);
        }
        else
        {
            this.uid = uid;
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        try
        {
            attendance = await AttendanceService.GetAttendanceByIdAsync(Id);
        }
        catch (Exception e)
        {
            message = e.Message;
        }
    }

    private async Task OKButtonOnClick()
    {
        try
        {
            isLoading = true;
            await AttendanceResponseService.SubmitResponseAsync(uid, Id, ResponseType.OK);
            NavigationManager.NavigateTo("");
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

    private async Task HoldButtonOnClick()
    {
        try
        {
            isLoading = true;
            await AttendanceResponseService.SubmitResponseAsync(uid, Id, ResponseType.HOLD);
            NavigationManager.NavigateTo("");
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

    private async Task NGButtonOnClick()
    {
        try
        {
            isLoading = true;
            await AttendanceResponseService.SubmitResponseAsync(uid, Id, ResponseType.NG);
            NavigationManager.NavigateTo("");
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
