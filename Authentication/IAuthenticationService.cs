using Firebase.Auth;

namespace Attendiia.Authenticaion;

public interface IAuthenticationService
{
    Task<LoginUserInfo?> LoginAsync(LoginModel loginModel);
    Task LogoutAsync();
}