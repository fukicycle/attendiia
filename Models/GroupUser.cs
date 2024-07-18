namespace Attendiia.Models;

public sealed class GroupUser
{
    public GroupUser(string groupCode, string uid)
    {
        GroupCode = groupCode;
        Uid = uid;
    }
    public string GroupCode { get; }
    public string Uid { get; }
}
