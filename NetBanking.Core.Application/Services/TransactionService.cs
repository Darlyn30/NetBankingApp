using AutoMapper;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.Transaction;
using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Services
{
    public class TransactionService : GenericService<SaveTransactionViewModel, TransactionViewModel, Transaction>, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper) : base(transactionRepository, mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }
    }
}
