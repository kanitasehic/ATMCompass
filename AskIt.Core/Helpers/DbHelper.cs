using ATMCompass.Core.Configuration;
using ATMCompass.Core.Constants;
using Microsoft.AspNetCore.Identity;

namespace ATMCompass.Core.Helpers
{
    public static class DbHelper
    {
        public static async Task SeedAdminUser(UserManager<IdentityUser> userManager, AdminCredentials adminCredentials)
        {
            IList<IdentityUser> adminUsers = await userManager.GetUsersInRoleAsync(Roles.ADMIN_ROLE);

            if (!adminUsers.Any())
            {

                var admin = new IdentityUser
                {
                    Email = adminCredentials.Email,
                    UserName = adminCredentials.UserName
                };

                IdentityResult result = await userManager.CreateAsync(admin, adminCredentials.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Roles.ADMIN_ROLE);
                }
            }
        }

        public static async Task SeedAdminRole(RoleManager<IdentityRole> roleManager)
        {
            bool adminRoleExists = await roleManager.RoleExistsAsync(Roles.ADMIN_ROLE);
            if (!adminRoleExists)
            {
                var adminRole = new IdentityRole
                {
                    Name = Roles.ADMIN_ROLE,
                    NormalizedName = Roles.ADMIN_ROLE.ToUpper()
                };

                await roleManager.CreateAsync(adminRole);
            }
        }
    }
}
