using FindMyTeddy.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyTeddy.Repositories.Interfaces
{
    public interface ICharacteristicRepository : IRepository<Characteristic>
    {
        Task<IEnumerable<Characteristic>> GetByPetId(Guid petId);
    }
}
