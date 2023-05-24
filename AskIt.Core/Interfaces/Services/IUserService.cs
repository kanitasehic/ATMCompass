using AskIt.Core.Models.Users.Requests;
using AskIt.Core.Models.Users.Responses;

namespace AskIt.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> LoginUserAsync(UserLoginRequest request);
    }
}
