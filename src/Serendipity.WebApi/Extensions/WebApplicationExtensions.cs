using Microsoft.AspNetCore.Identity;
using Serendipity.Domain.Defaults;
using Serendipity.Infrastructure.Models;

namespace Serendipity.WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static async Task SeedDefaultUser(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var adminUser = await userManager.FindByEmailAsync(Users.AdministratorEmail);

        if (adminUser is null)
        {
            await userManager.CreateAsync(new User
            {
                Email = Users.AdministratorEmail,
                NormalizedEmail = Users.AdministratorEmail,
                UserName = Users.AdministratorEmail,
                NormalizedUserName = Users.AdministratorEmail
            });
        }

        var adminRole = await roleManager.FindByNameAsync(Roles.Admin);

        if (adminRole is null)
        {
            await roleManager.CreateAsync(new IdentityRole
            {
                Name = Roles.Admin,
                NormalizedName = Roles.Admin
            });
        }
        
        adminUser = await userManager.FindByEmailAsync(Users.AdministratorEmail);
        await userManager.AddToRoleAsync(adminUser, Roles.Admin);
    }
}