using Microsoft.AspNetCore.Components;

namespace Attendiia.Components;

public partial class Loader
{
    [Parameter]
    public bool IsFullScreen { get; set; } = false;
}
