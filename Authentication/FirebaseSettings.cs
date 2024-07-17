namespace Attendiia.Authenticaion;

public sealed class FirebaseSettings
{
    public FirebaseSettings(string firebaseApiKey, string firebaseAuthDomain)
    {
        FirebaseApiKey = firebaseApiKey;
        FirebaseAuthDomain = firebaseAuthDomain;
    }
    public string FirebaseApiKey { get; }
    public string FirebaseAuthDomain { get; }
}