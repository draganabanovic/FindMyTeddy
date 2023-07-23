using FindMyTeddy.Data.Entities;
using FindMyTeddy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyTeddy.Domain.Interfaces
{
   public interface IPetLastLocationService
    {
        Task<GenericDomainModel<PetLastLocationDomainModel>> GetByPetIdAsync(Guid petId);
        Task<GenericDomainModel<PetLastLocationDomainModel>> CreateAsync(PetLastLocationDomainModel newLastLocation);
        Task<GenericDomainModel<PetLastLocationDomainModel>> DeactivatePreviousLastLocationsAsync(Guid petId);
    }
}
