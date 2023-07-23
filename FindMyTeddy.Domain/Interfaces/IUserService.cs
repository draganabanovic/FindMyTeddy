using FindMyTeddy.Data.Entities;
using FindMyTeddy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyTeddy.Domain.Interfaces
{
    public interface IUserService
    {
        Task<GenericDomainModel<UserDomainModel>> GetAllAsync();
        Task<GenericDomainModel<UserDomainModel>> GetByIdAsync(Guid userId);
        Task<GenericDomainModel<UserDomainModel>> GetByPetIdAsync(Guid petId);
        Task<GenericDomainModel<UserDomainModel>> CreateAsync(UserDomainModel newUser);
        Task<GenericDomainModel<UserDomainModel>> UpdateAsync(UserDomainModel newUser);
        Task<GenericDomainModel<UserDomainModel>> DeleteAsync(Guid userId);
        Task<GenericDomainModel<UserDomainModel>> LoginAsync(string email,string password);

    }
}
