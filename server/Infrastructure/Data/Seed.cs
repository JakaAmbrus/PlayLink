using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data
{
    public static class Seed
    {
        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

            string[] roleNames = { "Member", "Moderator", "Admin" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var roleResult = await roleManager.CreateAsync(new AppRole { Name = roleName });

                    if (!roleResult.Succeeded)
                    {
                        throw new InvalidOperationException($"Error seeding '{roleName}' role");
                    }
                }
            }
        }
    }
}
