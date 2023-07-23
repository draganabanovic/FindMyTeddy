using FindMyTeddy.Data.Entities;
using FindMyTeddy.Data;
using FindMyTeddy.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FindMyTeddy.Repositories
{
    public class CharacteristicRepository : ICharacteristicRepository
    {
        private FindMyTeddyContext _findMyTeddyContext;
        public CharacteristicRepository (FindMyTeddyContext findMyTeddyContext)
        {
            _findMyTeddyContext = findMyTeddyContext;
        }

        public Characteristic Delete(object id)
        {
            var data = _findMyTeddyContext.Characteristics.Find((Guid)id);
            var deleted = _findMyTeddyContext.Characteristics.Remove(data);

            return deleted.Entity;
        }

        public async Task<IEnumerable<Characteristic>> GetAllAsync()
        {
            var data = await _findMyTeddyContext.Characteristics.ToListAsync();
            return data;
        }

        public async Task<Characteristic> GetByIdAsync(object id)
        {
            var data = await _findMyTeddyContext.Characteristics.Where(characteristic => characteristic.Id == (Guid)id).FirstOrDefaultAsync();
            return data;
        }

        public async Task<IEnumerable<Characteristic>> GetByPetId(Guid petId)
        {
            var data = await _findMyTeddyContext.Characteristics.Where(characteristic => characteristic.Pets.Any(pet => pet.Id == petId)).ToListAsync();
            return data;
        }

        public async Task<Characteristic> InsertAsync(Characteristic obj)
        {
            var data = await _findMyTeddyContext.Characteristics.AddAsync(obj);
            return data.Entity;
        }

        public async Task SaveAsync()
        {
            await _findMyTeddyContext.SaveChangesAsync();
        }

        public Characteristic Update(Characteristic obj)
        {
            var data = _findMyTeddyContext.Characteristics.Update(obj);
            return data.Entity;
        }
    }
}
