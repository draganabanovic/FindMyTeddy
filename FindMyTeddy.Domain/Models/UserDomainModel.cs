using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyTeddy.Domain.Models
{
    public class UserDomainModel
    {
        public Guid Id { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Street { get; set; }
        public string ProfilePicture { get; set; }
    }
}
