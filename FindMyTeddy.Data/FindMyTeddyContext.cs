using FindMyTeddy.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyTeddy.Data
{
    public class FindMyTeddyContext : IdentityDbContext<User, AppRole, Guid>
    {
        public DbSet<User> Users {get; set;}
        public DbSet<Pet> Pets { get; set;}
        public DbSet<PetLastLocation> PetLastLocations { get; set;}
        public DbSet<Characteristic> Characteristics { get; set;}

        public FindMyTeddyContext(DbContextOptions<FindMyTeddyContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
