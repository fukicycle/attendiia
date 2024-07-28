namespace Attendiia.Models;

public sealed class Attendance
{
    public Attendance(string title, string description, string email, bool updated = false)
    {
        Id = Guid.NewGuid().ToString();
        Title = title;
        Description = description;
        CreateDateTime = DateTime.Now;
        Email = email;
        IsUpdated = updated;
    }
    public string Id { get; }
    public string Title { get; }
    public string Description { get; }
    public DateTime CreateDateTime { get; }
    public string Email { get; }
    public bool IsUpdated { get; }
}
