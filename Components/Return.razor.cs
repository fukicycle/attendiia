using Microsoft.AspNetCore.Components;

namespace Attendiia.Components;

public partial class Return
{
    [Parameter]
    public string ReturnTo { get; set; } = null!;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (ReturnTo.StartsWith("/"))
        {
            throw new ArgumentException("Sorry, retrun url must not start with /.");
        }
    }

    private void OnClick()
    {
        NavigationManager.NavigateTo(ReturnTo);
    }
}
