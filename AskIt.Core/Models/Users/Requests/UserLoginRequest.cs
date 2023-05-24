using System.ComponentModel.DataAnnotations;

namespace AskIt.Core.Models.Users.Requests
{
    public class UserLoginRequest
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
