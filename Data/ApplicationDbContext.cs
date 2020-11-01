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
        }
    }
}
//    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
//    {

//        public ApplicationDbContext(
//            DbContextOptions options,
//            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
//        {
//        }
//        public DbSet<Car> Cars { get; set; }
//        public DbSet<Rent> Rents { get; set; }

//        //protected override void OnModelCreating(ModelBuilder modelBuilder)
//        //{
//        //    base.OnModelCreating(modelBuilder);
//        //    string ADMIN_ID = Guid.NewGuid().ToString();
//        //    string ROLE_ID = ADMIN_ID;
//        //    var hasher = new PasswordHasher<ApplicationUser>();
//        //    modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
//        //    {
//        //        Id = ADMIN_ID,
//        //        UserName = "admin@example.com",
//        //        NormalizedUserName = "admin@example.com".ToUpper(),
//        //        Email = "admin@example.com",
//        //        NormalizedEmail = "admin@example.com".ToUpper(),
//        //        EmailConfirmed = true,
//        //        PasswordHash = hasher.HashPassword(null, "SecretPassword111"),
//        //        SecurityStamp = string.Empty
//        //    });

//        //    modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
//        //    {
//        //        Id = ROLE_ID,
//        //        Name = "admin",
//        //        NormalizedName = "admin"
//        //    });

//        //    modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
//        //    {
//        //        RoleId = ROLE_ID,
//        //        UserId = ADMIN_ID
//        //    });
//        //}
//    }
//}
