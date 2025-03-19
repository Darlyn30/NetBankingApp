using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NetBanking.Infrastructure.Identity.Entities;
using NetBanking.Infrastructure.Identity.Seeds;

namespace NetBanking.Infrastructure.Identity
{
    public static class ServiceApplication
    {
        public static async Task AddIdentitySeeds(this IServiceProvider services)
        {
            #region "Identity Seeds"
            using (var scope = services.CreateScope())
            {
                var servicesScope = scope.ServiceProvider;

                try
                {
                    var userManager = servicesScope.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = servicesScope.GetRequiredService<RoleManager<IdentityRole>>();

                    await DefaultRoles.SeedAsync(userManager, roleManager);
                    await DefaultAdminUser.SeedAsync(userManager, roleManager);
                    await DefaultClientUser.SeedAsync(userManager, roleManager);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            #endregion
        }
    }
}
