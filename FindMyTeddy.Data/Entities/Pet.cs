using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyTeddy.Data.Entities
{
    public class Pet
    {
        public Guid Id { get; set; }
        public User Owner { get; set; }
        public ICollection<PetLastLocation> PetLastLocations { get; set; }
        public ICollection<Characteristic> Characteristics { get; set; }
        public string Name {get; set;}
        public string Type { get; set; }
        public string Breed { get; set; }
        public string Picture { get; set; }
        public bool LostStatus { get; set; }
        public string Description { get; set; }
        public bool IsSubscribed { get; set; }
        public DateTime DisappearanceDate { get; set; }
        public string ZipCode { get; set; }


    }
}
