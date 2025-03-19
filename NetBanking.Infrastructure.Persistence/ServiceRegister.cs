using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Infrastructure.Persistence.Contexts;
using NetBanking.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Infrastructure.Persistence
{
    public static class ServiceRegister
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Contexts
            services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            m => m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
            #endregion

            #region Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IBeneficiaryRepository, BeneficiaryRepository>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<ILoanRepository, LoanRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<ICreditCardRepository, CreditCardRepository>();
            services.AddTransient<ISavingsAccountRepository, SavingsAccountRepository>();
            #endregion
        }
    }
}
