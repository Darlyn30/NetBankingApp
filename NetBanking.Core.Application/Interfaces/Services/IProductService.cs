using NetBanking.Core.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<string> GenerateProductNumber();
        Task<ProductViewModel> GetAllProductsByClient(int clientId);
    }
}
