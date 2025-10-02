using InternetBanking.Domain.Entities;

namespace InternetBanking.Domain.Interfaces;

/// <summary>
/// Interface específica para repositório de transações
/// </summary>
public interface ITransactionRepository : IRepository<Transaction>
{
    Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId);
}