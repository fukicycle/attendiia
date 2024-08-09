namespace Attendiia.Models;

public sealed class AttendanceResponse
{
    public AttendanceResponse(string id, string uid, string attendanceId, string? comment, ResponseType type, DateTime responseDateTime)
    {
        Id = id;
        Uid = uid;
        AttendanceId = attendanceId;
        Comment = comment;
        Type = type;
        ResponseDateTime = responseDateTime;
    }
    public string Id { get; }
    public string Uid { get; }
    public string AttendanceId { get; }
    public string? Comment { get; }
    public ResponseType Type { get; set; }
    public DateTime ResponseDateTime { get; set; }
}
