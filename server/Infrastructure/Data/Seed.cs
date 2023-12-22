using Application.Interfaces;
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
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            string[] roleNames = { "Member", "Moderator", "Admin", "Guest" };
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
                /*if (roleName == "Guest")
                {
                    string[] usernames = { "testone", "testthree" };
                    foreach (var username in usernames)
                    {
                        var user = await userManager.FindByNameAsync(username);
                        if (user != null)
                        {
                            var hasRole = await userManager.IsInRoleAsync(user, roleName);
                            if (!hasRole)
                            {
                                var userRoleResult = await userManager.AddToRoleAsync(user, roleName);
                                if (!userRoleResult.Succeeded)
                                {
                                    throw new InvalidOperationException($"Error assigning '{roleName}' role to user '{username}'");
                                }
                            }
                        }
                    }
                }*/
            }
        }
    }
}
