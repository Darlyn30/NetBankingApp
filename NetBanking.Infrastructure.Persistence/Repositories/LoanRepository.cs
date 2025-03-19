using Microsoft.EntityFrameworkCore;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Domain.Entities;
using NetBanking.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Infrastructure.Persistence.Repositories
{
    public class LoanRepository : GenericRepository<Loan>, ILoanRepository
    {
        private readonly ApplicationContext _context;
        public LoanRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Loan> GetByAccountNumber(string accountNumber)
        {
            var loan = await _context.Loans.FirstOrDefaultAsync(b => b.Id == accountNumber);

            if (loan == null)
            {
                return null;
            }

            return loan;
        }

        public async Task<Loan> GetByAccountNumberLoggedUser(string accountNumber, int clientId)
        {
            var loan = await _context.Loans.FirstOrDefaultAsync(b => b.Id == accountNumber && b.ClientId == clientId);

            if (loan == null)
            {
                return null;
            }

            return loan;
        }
    }
}
