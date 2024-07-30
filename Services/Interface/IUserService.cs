using Attendiia.Authentication;
using Attendiia.Forms;

namespace Attendiia.Services.Interface;

public interface IUserService
{
    Task RegisterAsync(UserCreateForm userCreateForm);
    Task<LoginUserInfo> GetUserInfoAsync(string email);
}
