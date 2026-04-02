using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MediCore.Infrastructure.Data.Seed
{
    public static class RoleSeeder
    {
        private static readonly string[] DefaultRoles =
        {
            "Admin",
            "Doctor",
            "Pharmacist",
            "Nurse"
        };

        public static async Task SeedRolesAsync(
            RoleManager<IdentityRole> roleManager,
            ILogger logger,
            CancellationToken cancellationToken = default)
        {
            foreach (var roleName in DefaultRoles)
            {
                if (await roleManager.RoleExistsAsync(roleName))
                    continue;

                var result = await roleManager.CreateAsync(new IdentityRole(roleName));

                if (result.Succeeded)
                {
                    logger.LogInformation("Role '{Role}' created successfully.", roleName);
                }
                else
                {
                    logger.LogError(
                        "Failed to create role '{Role}'. Errors: {Errors}",
                        roleName,
                        string.Join(",", result.Errors.Select(e => e.Description))
                    );
                }
            }
        }
    }
}