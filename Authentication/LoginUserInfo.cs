namespace Attendiia.Authenticaion;

public sealed class LoginUserInfo
{
    public LoginUserInfo(string? firstName, string? lastName, string? displayName, string email, string? photoUrl)
    {
        FirstName = firstName;
        LastName = lastName;
        DisplayName = displayName;
        Email = email;
        PhotoUrl = photoUrl;
    }

    public string? FirstName { get; }

    public string? LastName { get; }

    public string? DisplayName { get; }

    public string Email { get; }

    public string? PhotoUrl { get; }
}