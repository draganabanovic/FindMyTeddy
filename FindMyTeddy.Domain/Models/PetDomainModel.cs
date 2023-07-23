using FindMyTeddy.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyTeddy.Domain.Models
{
    public class PetDomainModel
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public List<Guid>? CharacteristicIds { get; set; }
        public List<CharacteristicDomainModel> Characteristics { get; set; }
        public List<Guid> PetLastLocationIds { get; set; }
        public List<PetLastLocationDomainModel> PetLastLocations { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Breed { get; set; }
        public string Picture { get; set; }
        public bool LostStatus { get; set; }
        public string Description { get; set; }
        public bool IsSubscribed { get; set; }
        public DateTime? DisappearanceDate { get; set; }
        public string ZipCode { get; set; }
    }
}
