using FindMyTeddy.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyTeddy.Domain.Models
{
    public class PetLastLocationDomainModel
    {
        public Guid Id { get; set; }
        public Guid PetId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime LastLocationDateTime { get; set; }
        public bool IsRelevant { get; set; }
    }
}
