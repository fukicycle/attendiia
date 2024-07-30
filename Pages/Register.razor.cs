using Attendiia.Authentication;
using Attendiia.Forms;

namespace Attendiia.Pages;

public partial class Register
{
    private UserCreateForm userCreateForm = new UserCreateForm();
    private bool isLoading = false;

    private async Task OnValidRequest()
    {
        isLoading = true;
        await UserService.RegisterAsync(userCreateForm);
        LoginModel loginModel = new LoginModel(userCreateForm.Email, userCreateForm.Password);
        if (await AuthenticationService.LoginAsync(loginModel))
        {
            NavigationManager.NavigateTo("", true);
        }
    }
}
