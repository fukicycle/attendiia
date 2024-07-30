namespace Attendiia.Authentication;

public sealed class LoginUserInfo
{
    public LoginUserInfo(string uid, string email, string displayName)
    {
        Uid = uid;
        Email = email;
        DisplayName = displayName;
    }

    public string Uid { get; }
    public string Email { get; }
    public string DisplayName { get; }

}