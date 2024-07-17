using Firebase.Auth;
using Firebase.Auth.Providers;

namespace Attendiia.Authenticaion;

public sealed class FirebaseAuthenticationServices : IAuthenticationService
{
    private readonly FirebaseAuthenticationStateProvider _firebaseAuthenticationStateProvider;
    private readonly FirebaseAuthConfig _authConfig;

    public FirebaseAuthenticationServices(FirebaseAuthenticationStateProvider firebaseAuthenticationStateProvider)
    {
        _firebaseAuthenticationStateProvider = firebaseAuthenticationStateProvider;
        _authConfig = new FirebaseAuthConfig();
    }

    public async Task<LoginUserInfo?> LoginAsync(LoginModel loginModel)
    {
        try
        {
            FirebaseAuthProvider provider = new EmailProvider();
            FirebaseAuthClient client = new FirebaseAuthClient(_authConfig);
            UserCredential userCredential =
                await client.SignInWithEmailAndPasswordAsync(loginModel.UserId, loginModel.Password);
            string idToken = await userCredential.User.GetIdTokenAsync();
            await _firebaseAuthenticationStateProvider.NotifySignIn(loginModel.UserId, idToken);
            return new LoginUserInfo(
                userCredential.User.Info.FirstName,
                userCredential.User.Info.LastName,
                userCredential.User.Info.DisplayName,
                userCredential.User.Info.Email,
                userCredential.User.Info.PhotoUrl
            );
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task LogoutAsync()
    {
        FirebaseAuthClient client = new FirebaseAuthClient(_authConfig);
        client.SignOut();
        await _firebaseAuthenticationStateProvider.NotifySignOut();
    }
}