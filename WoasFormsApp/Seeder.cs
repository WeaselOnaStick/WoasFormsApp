using Microsoft.AspNetCore.Identity;
using WoasFormsApp.Data;

namespace WoasFormsApp
{
    public class Seeder
    {
        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            string[] roleNames = { "Admin" };
            string[] adminUsernames = { "admin", "admin1", "admin2" };
            
            
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetService<UserManager<WoasFormsAppUser>>();


            foreach (var roleName in roleNames)
                if (! await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole(roleName));

            foreach (var adminUsername in adminUsernames)
            {
                var adminUser = await userManager.FindByNameAsync(adminUsername);
                if ((adminUser != null) && (!await userManager.IsInRoleAsync(adminUser, "Admin")))
                    await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
