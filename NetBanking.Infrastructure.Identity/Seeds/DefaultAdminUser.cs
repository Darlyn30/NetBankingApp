using Microsoft.AspNetCore.Identity;
using NetBanking.Core.Application.Enums;
using NetBanking.Infrastructure.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Infrastructure.Identity.Seeds
{
    public static class DefaultAdminUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser defaultUser = new();
            defaultUser.UserName = "adminuser";
            defaultUser.Email = "adminuser@gmail.com";
            defaultUser.Name = "John";
            defaultUser.LastName = "Doe";
            defaultUser.EmailConfirmed = true;
            defaultUser.PhoneNumberConfirmed = true;
            defaultUser.IdentificationNumber = "153-5456547-8";
            defaultUser.IsActive = true;

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByNameAsync(defaultUser.UserName);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123P4$$w0rd!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                }
            }
        }
    }
}
