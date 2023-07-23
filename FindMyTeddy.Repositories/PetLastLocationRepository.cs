using FindMyTeddy.Data.Entities;
using FindMyTeddy.Data;
using FindMyTeddy.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace FindMyTeddy.Repositories
{
    public class PetLastLocationRepository : IPetLastLocationRepository
    {
        private FindMyTeddyContext _findMyTeddyContext;
        public PetLastLocationRepository (FindMyTeddyContext findMyTeddyContext)
        {
            _findMyTeddyContext = findMyTeddyContext;
        }

        public async Task DeactivateAllPreviousAsync(Guid petId)
        {
            var data = await _findMyTeddyContext.PetLastLocations
                .Where(petLastLocation => petLastLocation.Pet.Id == petId && petLastLocation.IsRelevant == true).ToListAsync();
            if (data != null)
            {
                foreach (var row in data)
                {
                    row.IsRelevant = false;
                }
                _findMyTeddyContext.SaveChanges();
            }
        }

        public PetLastLocation Delete(object id)
        {
            var data = _findMyTeddyContext.PetLastLocations.Find((Guid)id);
            var deleted = _findMyTeddyContext.PetLastLocations.Remove(data);

            return deleted.Entity;
        }

        public async Task<IEnumerable<PetLastLocation>> GetAllAsync()
        {
            var data = await _findMyTeddyContext.PetLastLocations.ToListAsync();
            return data;
        }

        public async Task<PetLastLocation> GetByIdAsync(object id)
        {
            var data = await _findMyTeddyContext.PetLastLocations.Where(petLastLocation => petLastLocation.Id == (Guid)id).OrderByDescending(c => c.LastLocationDateTime).FirstOrDefaultAsync();
            return data;
        }

        public async Task<IEnumerable <PetLastLocation>> GetRelevantByPetIdAsync(Guid petId)
        {
            var data= await _findMyTeddyContext.PetLastLocations.Where(petLastLocation=>petLastLocation.Pet.Id == petId && petLastLocation.IsRelevant==true)
                .OrderByDescending(c => c.LastLocationDateTime).ToListAsync();
            return data;
        }

        public async Task<PetLastLocation> InsertAsync(PetLastLocation obj)
        {
            var data = await _findMyTeddyContext.PetLastLocations.AddAsync(obj);
            return data.Entity;
        }

        public async Task SaveAsync()
        {
            await _findMyTeddyContext.SaveChangesAsync();
        }

        public PetLastLocation Update(PetLastLocation obj)
        {
            var data = _findMyTeddyContext.PetLastLocations.Update(obj);
            return data.Entity;
        }
    }
}
