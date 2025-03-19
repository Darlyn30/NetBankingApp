using Microsoft.AspNetCore.Identity;
using NetBanking.Core.Application.Enums;
using NetBanking.Infrastructure.Identity.Entities;

namespace NetBanking.Infrastructure.Identity.Seeds
{
    public static class DefaultClientUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser defaultUser = new();
            defaultUser.UserName = "clientuser";
            defaultUser.Email = "clientuser@gmail.com";
            defaultUser.Name = "Heung Min";
            defaultUser.LastName = "Son";
            defaultUser.EmailConfirmed = true;
            defaultUser.PhoneNumberConfirmed = true;
            defaultUser.IdentificationNumber = "215-3254478-5";
            defaultUser.IsActive = true;

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByNameAsync(defaultUser.UserName);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123P4$$w0rd!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Client.ToString());
                }
            }
        }
    }
}
