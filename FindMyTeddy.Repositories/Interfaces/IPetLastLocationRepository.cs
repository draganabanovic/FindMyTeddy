using FindMyTeddy.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyTeddy.Repositories.Interfaces
{
    public interface IPetLastLocationRepository : IRepository<PetLastLocation>
    {
        Task<IEnumerable <PetLastLocation>> GetRelevantByPetIdAsync(Guid petId);
        Task DeactivateAllPreviousAsync(Guid petId);
    }
}
