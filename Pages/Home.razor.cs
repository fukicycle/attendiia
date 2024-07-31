using System.Security.Claims;
using Attendiia.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Attendiia.Pages;

public partial class Home
{
    [CascadingParameter]
    public Task<AuthenticationState> AuthenticationState { get; set; } = null!;
    private List<Attendance> attendances = new List<Attendance>();
    private bool isLoading = false;

    protected override async Task OnInitializedAsync()
    {
        isLoading = true;
        var authenticationState = await AuthenticationState;
        string? groupCode = authenticationState.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.UserData)?.Value;
        if (groupCode == default)
        {
            NavigationManager.NavigateTo("", true);
        }
        else
        {
            attendances = await AttendanceService.GetAttendancesByGroupCodeAsync(groupCode);
            await base.OnInitializedAsync();
        }
        isLoading = false;
    }
}
