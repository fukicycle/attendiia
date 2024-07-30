using Attendiia.Authentication;
using Attendiia.Components;
using Attendiia.Forms;
using Attendiia.Services.Interface;
using Blazored.LocalStorage;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin.Auth;

namespace Attendiia.Services;

public sealed class UserService : IUserService
{

    private readonly ILogger<UserService> _logger;
    private readonly FirebaseAuthConfig _authConfig;
    private readonly ILocalStorageService _localStorageService;

    public UserService(
        FirebaseAuthenticationSettings firebaseSettings,
        ILogger<UserService> logger,
        ILocalStorageService localStorageService)
    {
        _authConfig = new FirebaseAuthConfig
        {
            ApiKey = firebaseSettings.FirebaseApiKey,
            AuthDomain = firebaseSettings.FirebaseAuthDomain,
            Providers = new FirebaseAuthProvider[] {
                new EmailProvider(),
                new GoogleProvider()
            }
        };
        _logger = logger;
        _localStorageService = localStorageService;
    }
    public async Task<LoginUserInfo> GetUserInfoAsync(string email)
    {
        LoginUserInfo? loginUserInfo = await _localStorageService.GetItemAsync<LoginUserInfo>(LocalStorageKey.USER_INFO);
        if (loginUserInfo == null)
        {
            throw new Exception("Can not get login user information.");
        }
        return loginUserInfo;
    }

    public async Task RegisterAsync(UserCreateForm userCreateForm)
    {
        FirebaseAuthClient client = new FirebaseAuthClient(_authConfig);
        await client.CreateUserWithEmailAndPasswordAsync(
            userCreateForm.Email, userCreateForm.Password, userCreateForm.DisplayName);
    }
}
