using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Attendiia.Components;

public partial class GroupChecker
{
    private bool isLoading = true;
    private bool isNew = false;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = null!;

    [CascadingParameter]
    public Task<AuthenticationState> authenticationState { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        AuthenticationState state = await authenticationState;
        string? email = state.User.Identity?.Name;
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException($"{nameof(email)} is null or empty.");
        }
        UserGroupContainer.Groups.AddRange(await GroupService.GetGroupsByEmailAsync(email));
        isNew = !UserGroupContainer.Groups.Any();
        isLoading = false;
    }
}
