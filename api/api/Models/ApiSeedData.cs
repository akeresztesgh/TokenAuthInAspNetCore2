using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Utils;

namespace api.Models
{
    public class ApiDbSeedData
    {
        public ApiDbSeedData(UserManager<ApplicationUser> userManager)
        {

        }

        public static async Task Seed(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRolesAndClaims(userManager, roleManager);
            await SeedAdmin(userManager);
        }

        private static async Task SeedRolesAndClaims(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            if (!await roleManager.RoleExistsAsync("admin"))
            {
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = "admin"
                });
            }

            if (!await roleManager.RoleExistsAsync("user"))
            {
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = "user"
                });
            }


            var adminRole = await roleManager.FindByNameAsync("admin");
            var adminRoleClaims = await roleManager.GetClaimsAsync(adminRole);

            if (!adminRoleClaims.Any(x => x.Type == "manage_user"))
            {
                await roleManager.AddClaimAsync(adminRole, new System.Security.Claims.Claim("manage_user", "true"));
            }
            if (!adminRoleClaims.Any(x => x.Type == Extensions.AdminClaim))
            {
                await roleManager.AddClaimAsync(adminRole, new System.Security.Claims.Claim(Extensions.AdminClaim, "true"));
            }

            var userRole = await roleManager.FindByNameAsync("user");
            var userRoleClaims = await roleManager.GetClaimsAsync(userRole);
            if (!userRoleClaims.Any(x => x.Type == "user"))
            {
                await roleManager.AddClaimAsync(userRole, new System.Security.Claims.Claim("user", "true"));
            }
        }

        private static async Task SeedAdmin(UserManager<ApplicationUser> userManager)
        {
            var u = await userManager.FindByNameAsync("admin");
            if (u == null)
            {
                u = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@nothing.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    IsEnabled = true,
                    FirstName = "admin",
                    LastName = "user"
                };
                var x = await userManager.CreateAsync(u, "Admin1234!");
            }
            var uc = await userManager.GetClaimsAsync(u);
            if (!uc.Any(x => x.Type == "phone"))
            {
                await userManager.AddClaimAsync(u, new System.Security.Claims.Claim("phone", "867-5309"));
            }

            if (!uc.Any(x => x.Type == Extensions.AdminClaim))
            {
                await userManager.AddClaimAsync(u, new System.Security.Claims.Claim(Extensions.AdminClaim, true.ToString()));
            }
        }
    }
}
