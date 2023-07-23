using FindMyTeddy.Data;
using FindMyTeddy.Data.Entities;
using FindMyTeddy.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyTeddy.Repositories
{
    public class UserRepository : IUserRepository
    {

        private FindMyTeddyContext _findMyTeddyContext;
        public UserRepository(FindMyTeddyContext findMyTeddyContext)
        {
            _findMyTeddyContext = findMyTeddyContext;
        }
        

        public User Delete(object id)
        {
            var data = _findMyTeddyContext.Users.Find((Guid)id);
            var deleted = _findMyTeddyContext.Users.Remove(data);

            return deleted.Entity;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var data = await _findMyTeddyContext.Users.ToListAsync();
            return data;
        }

        public async Task<User> GetByIdAsync(object id)
        {
            var data = await _findMyTeddyContext.Users.Where(user => user.Id == (Guid)id).FirstOrDefaultAsync();
            return data;
        }

        public async Task<User> GetByPetIdAsync(Guid petId)
        {
            var data = await _findMyTeddyContext.Users.Where(user => user.Pets.Any(pet=>pet.Id==petId)).FirstOrDefaultAsync();
            return data;
        }

        public async Task<User> InsertAsync(User obj)
        {
            var data = await _findMyTeddyContext.Users.AddAsync(obj);
            return data.Entity;
        }

        public async Task SaveAsync()
        {
            await _findMyTeddyContext.SaveChangesAsync();
        }

        public User Update(User obj)
        {
            var data = _findMyTeddyContext.Users.Update(obj);
            return data.Entity;
        }
    }
}
