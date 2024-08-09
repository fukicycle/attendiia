namespace Attendiia.Models;

public sealed class AttendanceResponseItem
{
    public AttendanceResponseItem(
        string id,
        string title,
        string description,
        DateTime createDateTime,
        string authorDisplayName,
        string groupCode,
        bool updated,
        string? comment,
        DateTime responseDateTime)
    {
        Id = id;
        Title = title;
        Description = description;
        CreateDateTime = createDateTime;
        AuthorDisplayName = authorDisplayName;
        GroupCode = groupCode;
        IsUpdated = updated;
        Comment = comment;
        ResponseDateTime = responseDateTime;
    }
    public string Id { get; }
    public string Title { get; }
    public string Description { get; }
    public DateTime CreateDateTime { get; }
    public string AuthorDisplayName { get; }
    public string GroupCode { get; }
    public bool IsUpdated { get; }
    public string? Comment { get; }
    public DateTime ResponseDateTime { get; }
}
