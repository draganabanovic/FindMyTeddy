using System.ComponentModel.DataAnnotations;

namespace FindMyTeddy.API.Models
{
    public class LoginUserModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
