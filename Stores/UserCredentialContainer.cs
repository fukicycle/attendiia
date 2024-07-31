namespace Attendiia.Stores;

public sealed class UserCredentialContainer
{
    public string IdToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
