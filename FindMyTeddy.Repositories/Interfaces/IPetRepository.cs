using FindMyTeddy.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyTeddy.Repositories.Interfaces
{
    public interface IPetRepository : IRepository<Pet>
    {
        Task<IEnumerable<Pet>> GetByUserIdAsync(Guid userId); 
        Task<IEnumerable<Pet>> GetLost();
        Task<IEnumerable<Pet>> GetLostFromDate(DateTime date);
        Task<IEnumerable<Pet>> GetLostForZipCode (string zipCode);
    }
}
