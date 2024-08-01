using System.Security.Claims;
using Attendiia.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Attendiia.Pages;

public partial class Home
{
    [CascadingParameter]
    public Task<AuthenticationState> AuthenticationState { get; set; } = null!;
    private List<AttendanceResponseItem> responsedAttendances = new List<AttendanceResponseItem>();
    private List<AttendanceUnresponseItem> unresponsedAttendances = new List<AttendanceUnresponseItem>();
    private bool isLoading = false;

    protected override async Task OnInitializedAsync()
    {
        isLoading = true;
        var authenticationState = await AuthenticationState;
        string? groupCode = authenticationState.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.UserData)?.Value;
        string? uid = authenticationState.User.Identity?.Name;
        if (groupCode == default || string.IsNullOrEmpty(uid))
        {
            NavigationManager.NavigateTo("", true);
        }
        else
        {
            responsedAttendances = await AttendanceService.GetResponseAttendancesAsync(groupCode, uid);
            unresponsedAttendances = await AttendanceService.GetUnresponseAttendancesAsync(groupCode, uid);
            await base.OnInitializedAsync();
        }
        isLoading = false;
    }

    private void AddButtonOnClick()
    {
        NavigationManager.NavigateTo("create/attendance");
    }

    private void ListItemOnClick(string id)
    {
        NavigationManager.NavigateTo($"attendance/detail?id={id}");
    }
}
