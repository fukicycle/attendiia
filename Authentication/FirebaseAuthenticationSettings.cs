namespace Attendiia.Authentication;

public sealed class FirebaseAuthenticationSettings
{
    public FirebaseAuthenticationSettings(string firebaseApiKey, string firebaseAuthDomain)
    {
        FirebaseApiKey = firebaseApiKey;
        FirebaseAuthDomain = firebaseAuthDomain;
    }
    public string FirebaseApiKey { get; }
    public string FirebaseAuthDomain { get; }
}