using System.ComponentModel.DataAnnotations;

namespace FindMyTeddy.API.Models
{
    public class UpdateUserModel : CreateUserModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string ProfilePicture { get; set; }
    }
}
