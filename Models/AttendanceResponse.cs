namespace Attendiia.Models;

public sealed class AttendanceResponse
{
    public AttendanceResponse(string id, string uid, string attendanceId, ResponseType type, DateTime responseDateTime)
    {
        Id = id;
        Uid = uid;
        AttendanceId = attendanceId;
        Type = type;
        ResponseDateTime = responseDateTime;
    }
    public string Id { get; }
    public string Uid { get; }
    public string AttendanceId { get; }
    public ResponseType Type { get; set; }
    public DateTime ResponseDateTime { get; set; }
}
