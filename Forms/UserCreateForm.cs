using System.ComponentModel.DataAnnotations;

namespace Attendiia.Forms;

public sealed class UserCreateForm
{
    [Required(ErrorMessage = "入力してください"), MaxLength(15, ErrorMessage = "15文字以内で入力してください")]
    public string DisplayName { get; set; } = null!;
    [Required(ErrorMessage = "入力してください")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "入力してください"), MinLength(8, ErrorMessage = "8文字以上にしてください")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    [Required(ErrorMessage = "入力してください"), MinLength(8, ErrorMessage = "8文字以上にしてください")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "パスワードが一致しません")]
    public string RePassword { get; set; } = null!;
}
