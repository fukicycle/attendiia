using Firebase.Auth;
using Firebase.Auth.Providers;

namespace Attendiia.Authentication;

public sealed class FirebaseAuthenticationService : IAuthenticationService
{
    private readonly FirebaseAuthenticationStateProvider _firebaseAuthenticationStateProvider;
    private readonly FirebaseAuthConfig _authConfig;

    public FirebaseAuthenticationService(FirebaseAuthenticationStateProvider firebaseAuthenticationStateProvider, FirebaseSettings firebaseSettings)
    {
        _firebaseAuthenticationStateProvider = firebaseAuthenticationStateProvider;
        _authConfig = new FirebaseAuthConfig
        {
            ApiKey = firebaseSettings.FirebaseApiKey,
            AuthDomain = firebaseSettings.FirebaseAuthDomain,
            Providers = new FirebaseAuthProvider[] {
                new EmailProvider(),
                new GoogleProvider()
            }
        };
    }

    public async Task<bool> LoginAsync(LoginModel loginModel)
    {
        try
        {
            FirebaseAuthProvider provider = new EmailProvider();
            FirebaseAuthClient client = new FirebaseAuthClient(_authConfig);
            UserCredential userCredential =
                await client.SignInWithEmailAndPasswordAsync(loginModel.UserId, loginModel.Password);
            string idToken = await userCredential.User.GetIdTokenAsync();
            LoginUserInfo loginUserInfo = new LoginUserInfo(
                userCredential.User.Info.FirstName,
                userCredential.User.Info.LastName,
                userCredential.User.Info.DisplayName,
                userCredential.User.Info.Email,
                userCredential.User.Info.PhotoUrl
            );
            await _firebaseAuthenticationStateProvider.NotifySignIn(loginUserInfo, idToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        FirebaseAuthClient client = new FirebaseAuthClient(_authConfig);
        client.SignOut();
        await _firebaseAuthenticationStateProvider.NotifySignOut();
    }
}