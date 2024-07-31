using System.ComponentModel.DataAnnotations;

namespace Attendiia.Forms;

public sealed class AttendanceCreateForm
{
    [Required(ErrorMessage = "タイトルを入力してください。"), MaxLength(20, ErrorMessage = "タイトルは20文字以内にしてください。")]
    public string Title { get; set; } = string.Empty;
    [Required(ErrorMessage = "説明を入力してください。"), MaxLength(200, ErrorMessage = "説明は200文字以内にしてください。")]
    public string Description { get; set; } = string.Empty;
    [Required]
    public string AuthorDisplayName { get; set; } = string.Empty;
    [Required]
    public string GroupCode { get; set; } = null!;
}
