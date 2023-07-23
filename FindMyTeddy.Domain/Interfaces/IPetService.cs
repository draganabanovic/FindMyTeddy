using FindMyTeddy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyTeddy.Domain.Interfaces
{
   public interface IPetService
    {
        Task<GenericDomainModel<PetDomainModel>> GetByIdAsync(Guid petId);
        Task<GenericDomainModel<PetDomainModel>> GetAllAsync();
        Task<GenericDomainModel<PetDomainModel>>GetByOwnerIdAsync(Guid ownerId);
        Task<GenericDomainModel<PetDomainModel>> GetAllLostAsync();
        Task<GenericDomainModel<PetDomainModel>> GetLostFromDateAsync(DateTime fromDate);
        Task<GenericDomainModel<PetDomainModel>>CreateAsync(PetDomainModel newPet);
        Task<GenericDomainModel<PetDomainModel>>ChangeStatusAsync(Guid petId, bool status);
        Task<GenericDomainModel<PetDomainModel>> ChangeAsync(PetDomainModel changedPet);
        Task<GenericDomainModel<PetDomainModel>> DeleteAsync(Guid petId);
        Task <GenericDomainModel<PetDomainModel>> GetLostForZipCode(string zipCode);
    }
}
