using Attendiia.Stores;
using Blazored.LocalStorage.JsonConverters;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin;
using FirebaseAdmin.Auth;

namespace Attendiia.Authentication;

public sealed class FirebaseAuthenticationService : IAuthenticationService
{
    private readonly FirebaseAuthenticationStateProvider _firebaseAuthenticationStateProvider;
    private readonly IFirebaseAuthClient _firebaseAuthClient;
    private readonly ILogger<FirebaseAuthenticationService> _logger;

    public FirebaseAuthenticationService(
        FirebaseAuthenticationStateProvider firebaseAuthenticationStateProvider,
        IFirebaseAuthClient firebaseAuthClient,
        ILogger<FirebaseAuthenticationService> logger)
    {
        _firebaseAuthenticationStateProvider = firebaseAuthenticationStateProvider;
        _firebaseAuthClient = firebaseAuthClient;
        _logger = logger;
    }

    public async Task<bool> LoginAsync(LoginModel loginModel)
    {
        try
        {
            FirebaseAuthProvider provider = new EmailProvider();
            UserCredential userCredential =
                await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(loginModel.Email, loginModel.Password);
            string idToken = await userCredential.User.GetIdTokenAsync();
            string refreshToken = userCredential.User.Credential.RefreshToken;
            LoginUserInfo loginUserInfo = new LoginUserInfo(
                userCredential.User.Uid,
                userCredential.User.Info.Email,
                userCredential.User.Info.DisplayName
            );
            await _firebaseAuthenticationStateProvider.NotifySignIn(loginUserInfo, idToken, refreshToken);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        _firebaseAuthClient.SignOut();
        await _firebaseAuthenticationStateProvider.NotifySignOut();
    }
}