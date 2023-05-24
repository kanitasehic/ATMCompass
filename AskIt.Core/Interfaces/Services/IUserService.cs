using ATMCompass.Core.Models.Users.Requests;
using ATMCompass.Core.Models.Users.Responses;

namespace ATMCompass.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> LoginUserAsync(UserLoginRequest request);
    }
}
