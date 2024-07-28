using Attendiia.Forms;
using Attendiia.Models;

namespace Attendiia.Services.Interface;

public interface IAttendanceService
{
    Task<string> CreateAttendanceAsync(AttendanceCreateForm attendanceFormData);
    Task<List<Attendance>> GetAttendancesAsync();
    Task<Attendance?> GetAttendanceByIdAsync(string id);
    Task UpdateAttendanceAsync(string id, AttendanceCreateForm attendanceFormData);
    Task DeleteAttendanceAsync(string id);
}
