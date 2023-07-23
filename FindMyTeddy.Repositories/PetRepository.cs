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
    public class PetRepository : IPetRepository
    {

        private FindMyTeddyContext _findMyTeddyContext;
        public PetRepository(FindMyTeddyContext findMyTeddyContext)
        {
            _findMyTeddyContext = findMyTeddyContext;
        }

        public Pet Delete(object id)
        {
            var data = _findMyTeddyContext.Pets.Find((Guid)id);
            var deleted = _findMyTeddyContext.Pets.Remove(data);

            return deleted.Entity;
        }

        public async Task<IEnumerable<Pet>> GetAllAsync()
        {
            var data = await _findMyTeddyContext.Pets
                .Include(pet => pet.Owner)
                .Include(pet => pet.Characteristics)
                .Include(pet => pet.PetLastLocations.Where(ll => ll.IsRelevant)
                                                    .OrderByDescending(d => d.LastLocationDateTime))
                .ToListAsync();
            return data;
        }

        public async Task<Pet> GetByIdAsync(object id)
        {
            var data = await _findMyTeddyContext.Pets.Where(pet => pet.Id == (Guid)id)
                .Include(pet => pet.Owner)
                .Include(pet => pet.Characteristics)
                .Include(pet => pet.PetLastLocations.Where(ll => ll.IsRelevant)
                                                    .OrderByDescending(d => d.LastLocationDateTime))
                .FirstOrDefaultAsync();
            return data;
        }

        public async Task<IEnumerable<Pet>> GetByUserIdAsync(Guid userId)
        {
            var data = await _findMyTeddyContext.Pets.Where(pet => pet.Owner.Id == userId)
                .Include(pet => pet.Owner)
                .Include(pet => pet.Characteristics)
                .Include(pet => pet.PetLastLocations.Where(ll => ll.IsRelevant)
                                                    .OrderByDescending(d => d.LastLocationDateTime))
                .OrderByDescending(pet => pet.DisappearanceDate)
                .ToListAsync();
            return data;
        }

        public async Task<IEnumerable<Pet>> GetLost()
        {
            var data= await _findMyTeddyContext.Pets.Where(pet=>pet.LostStatus==true)
                .Include(pet => pet.Owner)
                .Include(pet => pet.Characteristics)
                .Include(pet => pet.PetLastLocations.Where(ll => ll.IsRelevant)
                                                    .OrderByDescending(d => d.LastLocationDateTime))
                .OrderByDescending(pet => pet.DisappearanceDate)
                .ToListAsync();
            return data;

        }

        public async Task<IEnumerable<Pet>> GetLostFromDate(DateTime date)
        {
            var data= await _findMyTeddyContext.Pets.Where(pet=>pet.LostStatus==true && pet.DisappearanceDate.Date >= date.Date)
                .Include(pet => pet.Owner)
                .Include(pet => pet.Characteristics)
                .Include(pet => pet.PetLastLocations)
                .OrderByDescending(pet => pet.DisappearanceDate)
                .ToListAsync();
            return data;
        }

        public async Task<IEnumerable<Pet>> GetLostForZipCode(string zipCode)
        {
            var data = await _findMyTeddyContext.Pets.Where(pet => pet.LostStatus == true && pet.ZipCode==zipCode)
                .Include(pet => pet.Owner)
                .Include(pet => pet.Characteristics)
                .Include(pet => pet.PetLastLocations)
                .OrderByDescending(pet => pet.DisappearanceDate)
                .ToListAsync();
            return data;
        }

        public async Task<Pet> InsertAsync(Pet obj)
        {
            var data = await _findMyTeddyContext.Pets.AddAsync(obj);
            return data.Entity;
        }

        public async Task SaveAsync()
        {
            await _findMyTeddyContext.SaveChangesAsync();
        }

        public Pet Update(Pet obj)
        {
            var data = _findMyTeddyContext.Pets.Update(obj);
            return data.Entity;
        }

    }
}
