using System.ComponentModel.DataAnnotations;

namespace FindMyTeddy.API.Models
{
    public class CreateCharacteristicModel
    {
        [Required]
        public string Name { get; set; }
    }
}
