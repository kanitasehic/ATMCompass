using AskIt.Core.Configuration;
using AskIt.Core.Constants;
using AskIt.Core.Exceptions;
using AskIt.Core.Interfaces.Services;
using AskIt.Core.Models.Users.Requests;
using AskIt.Core.Models.Users.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AskIt.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AuthConfiguration _authConfiguration;

        public UserService(UserManager<IdentityUser> userManager, AuthConfiguration authConfiguration)
        {
            _userManager = userManager;
            _authConfiguration = authConfiguration;
        }

        public async Task<UserManagerResponse> LoginUserAsync(UserLoginRequest request)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                throw new NotAuthenticatedException(string.Format(ErrorMessages.USER_NOT_FOUND, request.Email));
            }

            bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordCorrect)
            {
                throw new NotAuthenticatedException(ErrorMessages.INCORRECT_PASSWORD);
            }

            JwtSecurityToken token = await GenerateTokenAsync(user);

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                Message = SuccessMessages.USER_SUCCESSFULLY_LOGGED_IN,
                Token = tokenAsString,
                IsSuccess = true,
                ExpireDate = token.ValidTo
            };
        }

        private async Task<JwtSecurityToken> GenerateTokenAsync(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(Claims.EMAIL_ADDRESS, user.Email),
                new Claim(Claims.USER_IDENTIFIER, user.Id),
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(Claims.USER_ROLE, role));
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authConfiguration.IssuerSigningKey));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _authConfiguration.Issuer,
                audience: _authConfiguration.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
