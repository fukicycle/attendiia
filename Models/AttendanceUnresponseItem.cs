namespace Attendiia.Models;

public sealed class AttendanceUnresponseItem
{
    public AttendanceUnresponseItem(string id, string title, string description, DateTime createDateTime, string authorDisplayName, string groupCode, bool updated)
    {
        Id = id;
        Title = title;
        Description = description;
        CreateDateTime = createDateTime;
        AuthorDisplayName = authorDisplayName;
        GroupCode = groupCode;
        IsUpdated = updated;
    }
    public string Id { get; }
    public string Title { get; }
    public string Description { get; }
    public DateTime CreateDateTime { get; }
    public string AuthorDisplayName { get; }
    public string GroupCode { get; }
    public bool IsUpdated { get; }
}
