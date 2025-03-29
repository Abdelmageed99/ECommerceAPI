using ECommerceAPI.Modules.Users.Models;
using Microsoft.AspNetCore.Identity;

namespace ECommerceAPI.Shared.RoleInitializer
{
    public class RoleInitializerHelper
    {
        public static async Task SeedRoles(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager
            )
        {
            List<string> roles = new List<string>();

            roles.Add("Admin");
            roles.Add("Customer");

            foreach (var role in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminEmail = "admin@ecomm.com";
            var adminFullName = "Admin";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser { Email = adminEmail, UserName = "Admin", FullName = adminFullName };

                var createAdmin = await userManager.CreateAsync(newAdmin, "Admin@123");

                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }

            }
        }
    }
}
