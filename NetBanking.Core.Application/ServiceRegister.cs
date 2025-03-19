using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Services;

namespace NetBanking.Core.Application
{
    public static class ServiceRegister
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            #region Services
            services.AddTransient(typeof(IGenericService<,,>), typeof(GenericService<,,>));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IBeneficiaryService, BeneficiaryService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddScoped<ILoanService, LoanService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddScoped<ISavingsAccountService, SavingsAccountService>();
            services.AddScoped<ICreditCardService, CreditCardService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddTransient<ITransfersService, TransfersService>();
            services.AddTransient<IPaymentService, PaymentService>();
            #endregion
        }
    }
}
