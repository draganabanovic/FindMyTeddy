using FindMyTeddy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyTeddy.Domain.Interfaces
{
    public interface ICharacteristicService
    {
        Task<GenericDomainModel<CharacteristicDomainModel>> GetByIdAsync(Guid characteristicId);
        Task<GenericDomainModel<CharacteristicDomainModel>> GetAllAsync();
        Task<GenericDomainModel<CharacteristicDomainModel>> CreateAsync(string characteristicName);
        Task<GenericDomainModel<CharacteristicDomainModel>> DeleteAsync(Guid characteristicId);
        Task<GenericDomainModel<CharacteristicDomainModel>> GetByPetIdAsync(Guid petId);

    }
}
