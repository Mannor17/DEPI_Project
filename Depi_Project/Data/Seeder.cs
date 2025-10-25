using Microsoft.AspNetCore.Identity;

namespace Depi_Project.Data
{
    public class Seeder
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = new[] { "Admin", "GymOwner", "User" };
            foreach (var r in roles)
            {
                if (!await roleManager.RoleExistsAsync(r))
                    await roleManager.CreateAsync(new IdentityRole(r));
            }
        }
    }
}
