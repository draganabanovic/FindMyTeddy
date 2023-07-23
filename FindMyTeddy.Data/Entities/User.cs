using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyTeddy.Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        //public Guid Id { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string ProfilePicture { get; set; }
        public ICollection<Pet> Pets { get; set; }

    }
}
