using Attendiia.Forms;
using Attendiia.Models;
using Attendiia.Services.Interface;

namespace Attendiia.Services;

public sealed class AttendanceService : IAttendanceService
{
    private readonly IFirebaseDatabaseService _firebaseDatabaseService;
    public AttendanceService(IFirebaseDatabaseService firebaseDatabaseService)
    {
        _firebaseDatabaseService = firebaseDatabaseService;
    }

    public async Task<string> CreateAttendanceAsync(AttendanceCreateForm attendanceFormData)
    {
        Attendance attendance = new Attendance(
            Guid.NewGuid().ToString(),
            attendanceFormData.Title,
            attendanceFormData.Description,
            attendanceFormData.AuthorEmail);
        await _firebaseDatabaseService.AddItemAsync(
            FirebaseDatabaseKeys.ATTENDANCE_PATH,
            attendance.Id,
            attendance);
        return attendance.Id;
    }

    public async Task DeleteAttendanceAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentException($"{nameof(id)} is null or empty.");
        }
        if (!await IsExistsAsync(id))
        {
            throw new ArgumentException($"{nameof(id)} is not found.");
        }
        await _firebaseDatabaseService.DeleteItemAsync(
            FirebaseDatabaseKeys.ATTENDANCE_PATH, id);
    }

    public async Task<Attendance?> GetAttendanceByIdAsync(string id)
    {
        return await _firebaseDatabaseService.GetItemAsync<Attendance?>(
            FirebaseDatabaseKeys.ATTENDANCE_PATH, id);
    }

    public async Task<List<Attendance>> GetAttendancesAsync()
    {
        List<Attendance> attendances = await _firebaseDatabaseService.GetItemsAsync<Attendance>(
            FirebaseDatabaseKeys.ATTENDANCE_PATH);
        return attendances.OrderByDescending(a => a.CreateDateTime).ToList();
    }

    public async Task UpdateAttendanceAsync(string id, AttendanceCreateForm attendanceFormData)
    {
        if (!await IsExistsAsync(id))
        {
            throw new ArgumentException($"{nameof(id)} is not found.");
        }
        //更新すると確認済みを解除したいため新規で追加する。
        Attendance attendance = new Attendance(
            id,
            attendanceFormData.Title,
            attendanceFormData.Description,
            attendanceFormData.AuthorEmail,
            true);//更新フラグ
        await _firebaseDatabaseService.UpdateItemAsync(
            FirebaseDatabaseKeys.ATTENDANCE_PATH, attendance.Id, attendance);
        //古い情報は削除する。
        await DeleteAttendanceAsync(id);
    }

    private async Task<bool> IsExistsAsync(string id)
    {
        Attendance? attendance = await GetAttendanceByIdAsync(id);
        return attendance != null;
    }
}
