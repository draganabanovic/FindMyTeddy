using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyTeddy.Data.Entities
{
    public class Characteristic
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Pet> Pets { get; set; }
    }
}
