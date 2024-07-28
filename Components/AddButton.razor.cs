using Microsoft.AspNetCore.Components;

namespace Attendiia.Components;

public partial class AddButton
{
    [Parameter]
    public EventCallback OnClick { get; set; }
}
