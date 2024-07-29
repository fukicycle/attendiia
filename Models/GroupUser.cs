namespace Attendiia.Models;

public sealed class GroupUser
{
    public GroupUser(string id, string groupCode, string email)
    {
        Id = id;
        GroupCode = groupCode;
        Email = email;
    }
    public string Id { get; }
    public string GroupCode { get; }
    public string Email { get; }
}
