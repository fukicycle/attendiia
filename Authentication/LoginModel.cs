namespace Attendiia.Authenticaion;

public sealed class LoginModel
{
    public LoginModel(string userId, string password)
    {
        UserId = userId;
        Password = password;
    }
    public string UserId { get; }
    public string Password { get; }
}