using Firebase.Auth;

namespace Attendiia.Authentication;

public interface IAuthenticationService
{
    Task<bool> LoginAsync(LoginModel loginModel);
    Task LogoutAsync();
}