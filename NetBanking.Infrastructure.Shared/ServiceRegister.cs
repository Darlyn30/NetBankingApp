using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Domain.Settings;
using NetBanking.Infrastructure.Shared.Services;

namespace NetBanking.Infrastructure.Shared
{
    public static class ServiceRegister
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<MailSettings>(_config.GetSection("MailSettings"));
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}
