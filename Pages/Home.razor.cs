using Attendiia.Models;

namespace Attendiia.Pages;

public partial class Home
{
    private List<Attendance> attendances = new List<Attendance>();
    private bool isLoading = false;

    protected override async Task OnInitializedAsync()
    {
        isLoading = true;
        attendances = await AttendanceService.GetAttendancesAsync();
        await base.OnInitializedAsync();
        isLoading = false;
    }
}
