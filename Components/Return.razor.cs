using Microsoft.AspNetCore.Components;

namespace Attendiia.Components;

public partial class Return
{
    [Parameter]
    public string ReturnTo { get; set; } = null!;

    private void OnClick()
    {
        NavigationManager.NavigateTo(ReturnTo);
    }
}
