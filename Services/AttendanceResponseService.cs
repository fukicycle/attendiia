using Attendiia.Models;
using Attendiia.Services.Interface;

namespace Attendiia.Services;

public sealed class AttendanceResponseService : IAttendanceResponseService
{
    private readonly IFirebaseDatabaseService _firebaseDatabaseService;
    private readonly ILogger<AttendanceResponseService> _logger;
    public AttendanceResponseService(
        IFirebaseDatabaseService firebaseDatabaseService,
        ILogger<AttendanceResponseService> logger)
    {
        _firebaseDatabaseService = firebaseDatabaseService;
        _logger = logger;
    }
    public async Task<List<AttendanceResponse>> GetResponsesByUserIdAsync(string uid)
    {
        return await _firebaseDatabaseService.GetItemsAsync<AttendanceResponse>(
            FirebaseDatabaseKeys.ATTENDANCE_RESPONSE_PATH, nameof(AttendanceResponse.Uid), uid);
    }

    public async Task SubmitResponseAsync(string uid, string attendanceId, ResponseType responseType)
    {
        string id = Guid.NewGuid().ToString();
        AttendanceResponse attendanceResponse = new AttendanceResponse(id, uid, attendanceId, responseType, DateTime.Now);
        await _firebaseDatabaseService.AddItemAsync(
            FirebaseDatabaseKeys.ATTENDANCE_RESPONSE_PATH, id, attendanceResponse);
    }

    public async Task UpdateResponseAsync(string id, ResponseType responseType)
    {
        try
        {
            AttendanceResponse attendanceResponse =
            await _firebaseDatabaseService.GetItemAsync<AttendanceResponse>(
                FirebaseDatabaseKeys.ATTENDANCE_RESPONSE_PATH, id);
            attendanceResponse.Type = responseType;
            attendanceResponse.ResponseDateTime = DateTime.Now;
            await _firebaseDatabaseService.UpdateItemAsync(
                FirebaseDatabaseKeys.ATTENDANCE_RESPONSE_PATH, id, attendanceResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}
