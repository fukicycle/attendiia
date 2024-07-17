using Attendiia.Authentication;

namespace Attendiia.Pages;

public partial class SignIn
{
    private string userId { get; set; } = string.Empty;
    private string password { get; set; } = string.Empty;
    private string? _message = null;

    private async Task SignInButtonOnClick()
    {
        _message = null;
        LoginModel loginModel = new LoginModel(userId, password);
        bool result = await FirebaseAuthenticationService.LoginAsync(loginModel);
        if (result)
        {
            NavigationManager.NavigateTo("", true);
        }
        else
        {
            _message = "ユーザ名またはパスワードが違います。";
        }
    }

    private void RegisterButtonOnClick()
    {
        NavigationManager.NavigateTo("register");
    }
}
