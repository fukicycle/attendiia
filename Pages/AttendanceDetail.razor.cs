using Microsoft.AspNetCore.Components;

namespace Attendiia.Pages;

public partial class AttendanceDetail
{
    [Parameter]
    [SupplyParameterFromQuery(Name = "id")]
    public string Id { get; set; } = string.Empty;
}
