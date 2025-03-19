using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Services;
using NetBanking.Infrastructure.Identity.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetBanking.Infrastructure.Identity.Entities;
using NetBanking.Infrastructure.Identity.Services;

namespace NetBanking.Infrastructure.Identity
{
    public static class ServiceRegister
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Contexts
            services.AddDbContext<IdentityContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
                m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
            });
            #endregion

            #region Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User/RedirectIndex";
                options.AccessDeniedPath = "/User/AccessDenied";
            });

            services.AddAuthentication();
            #endregion

            #region Services
            services.AddTransient<IAccountService, AccountService>();
            #endregion
        }
    }
}
