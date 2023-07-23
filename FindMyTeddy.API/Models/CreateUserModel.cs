
using System.ComponentModel.DataAnnotations;

namespace FindMyTeddy.API.Models
{
    public class CreateUserModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string Phone { get; set; }
    }

    public class FileUserModel
    {
        public IFormFile? PictureFile { get; set; }
        public string UserData { get; set; }
    }
}
