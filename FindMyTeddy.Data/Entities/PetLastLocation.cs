using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyTeddy.Data.Entities
{
    public class PetLastLocation
    {
        public Guid Id { get; set; }
        public Pet Pet { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime LastLocationDateTime { get; set;}
        public bool IsRelevant { get; set; }

    }
    
}
