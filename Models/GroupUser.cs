namespace Attendiia.Models;

public sealed class GroupUser
{
    public GroupUser(string id, string groupCode, string uid)
    {
        Id = id;
        GroupCode = groupCode;
        Uid = uid;
    }
    public string Id { get; }
    public string GroupCode { get; }
    public string Uid { get; }
    public bool IsCurrent { get; set; }
}
