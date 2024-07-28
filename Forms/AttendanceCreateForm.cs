using System.ComponentModel.DataAnnotations;

namespace Attendiia.Forms;

public sealed class AttendanceCreateForm
{
    [Required(ErrorMessage = "タイトルを入力してください。"), MaxLength(50, ErrorMessage = "タイトルは50文字以内にしてください。")]
    public string Title { get; set; } = string.Empty;
    [Required(ErrorMessage = "説明を入力してください。"), MaxLength(200, ErrorMessage = "説明は200文字以内にしてください。")]
    public string Description { get; set; } = string.Empty;
    public string AuthorEmail { get; set; } = string.Empty;
}
