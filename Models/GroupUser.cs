namespace Attendiia.Models;

public sealed class GroupUser
{
    public GroupUser(string groupCode, string email)
    {
        GroupCode = groupCode;
        Email = email;
    }
    public string GroupCode { get; }
    public string Email { get; }
}
