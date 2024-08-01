using System.ComponentModel.DataAnnotations;

namespace Attendiia.Forms;

public sealed class GroupCreateForm
{
    [Required(ErrorMessage = "入力してください"), MaxLength(20, ErrorMessage = "20文字以内にしてください")]
    public string Name { get; set; } = string.Empty;
}
