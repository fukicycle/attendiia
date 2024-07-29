using Microsoft.AspNetCore.Components;

namespace Attendiia.Components;

public partial class Loader
{
    [Parameter]
    public bool IsFullScreen { get; set; } = false;
    [Parameter]
    public string? Message { get; set; }
}
