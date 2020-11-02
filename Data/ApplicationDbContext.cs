using CarRental.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Data
{
    public class ApplicationDbContext : KeyApiAuthorizationDbContext<ApplicationUser, ApplicationRole, int>
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(entity => { entity.ToTable(name: "User"); });
            modelBuilder.Entity<ApplicationRole>(entity => { entity.ToTable(name: "Role"); });
            modelBuilder.Entity<Rent>().HasOne(r => r.Car).WithMany().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
