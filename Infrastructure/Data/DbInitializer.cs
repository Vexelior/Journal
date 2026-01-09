using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        await context.Database.EnsureCreatedAsync();

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            var adminRole = new IdentityRole("Admin");
            await roleManager.CreateAsync(adminRole);
        }

        if (!await roleManager.RoleExistsAsync("User"))
        {
            var userRole = new IdentityRole("User");
            await roleManager.CreateAsync(userRole);
        }

        var adminUser = await userManager.FindByEmailAsync("asanderson1994s@gmail.com");
        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = "Alex",
                Email = "asanderson1994s@gmail.com",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Pass1234!");
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        var userUser = await userManager.FindByEmailAsync("testuser123@gmail.com");
        if (userUser == null)
        {
            userUser = new IdentityUser
            {
                UserName = "TestUser",
                Email = "testuser123@gmail.com",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(userUser, "Pass1234!");
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(userUser, "User");
            }
        }
    }
}
