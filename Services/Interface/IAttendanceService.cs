using Attendiia.Forms;
using Attendiia.Models;

namespace Attendiia.Services.Interface;

public interface IAttendanceService
{
    Task<string> CreateAttendanceAsync(AttendanceCreateForm attendanceFormData);
    Task<List<Attendance>> GetAttendancesByGroupCodeAsync(string groupCode);
    Task<Attendance> GetAttendanceByIdAsync(string id);
    Task UpdateAttendanceAsync(string id, AttendanceCreateForm attendanceFormData);
    Task DeleteAttendanceAsync(string id);
}
