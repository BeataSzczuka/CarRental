using CarRental.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarRental.Data
{
    public static class DataSeeder
    {
        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            seedRoles(roleManager);
            seedData(userManager);
        }
        private static void seedData(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByEmailAsync("admin@example.com").Result == null)
            {
                ApplicationUser administrator = new ApplicationUser();
                administrator.UserName = "admin@example.com";
                administrator.NormalizedUserName = "admin@example.com".ToUpper();
                administrator.Email = "admin@example.com";
                administrator.NormalizedEmail = "admin@example.com".ToUpper();
                administrator.EmailConfirmed = true;

            IdentityResult result = userManager.CreateAsync(administrator, "SecretPassword111!").Result;


                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(administrator, "Admin").Wait();
                } 
            }
            var r = userManager.GetUsersInRoleAsync("Admin").Result;
            Console.WriteLine(r);
        }

        private static void seedRoles(RoleManager<ApplicationRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "User";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
