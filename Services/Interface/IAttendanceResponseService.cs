using Attendiia.Models;

namespace Attendiia.Services.Interface;

public interface IAttendanceResponseService
{
    Task SubmitResponseAsync(string uid, string attendanceId, string? comment, ResponseType responseType);
    Task UpdateResponseAsync(string id, ResponseType responseType);
    Task<List<AttendanceResponse>> GetResponsesByUserIdAsync(string uid);
}
