using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        await context.Database.EnsureCreatedAsync();

        if (context.Users.Any())
        {
            return;
        }

        var adminRole = new IdentityRole("Admin");
        await roleManager.CreateAsync(adminRole);

        var adminUser = new IdentityUser
        {
            UserName = "asanderson1994s@gmail.com",
            Email = "asanderson1994s@gmail.com",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, "Pass1234!");
        
        if (result.Succeeded && !string.IsNullOrEmpty(adminRole.Name))
        {
            await userManager.AddToRoleAsync(adminUser, adminRole.Name);
        }
    }
}
