namespace Attendiia.Models;

public sealed class Group
{
    public Group(string groupCode, string name)
    {
        GroupCode = groupCode;
        Name = name;
    }
    public string GroupCode { get; }
    public string Name { get; }
}
