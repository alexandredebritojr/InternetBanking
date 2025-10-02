using InternetBanking.Domain.Entities;
using InternetBanking.Domain.Interfaces;
using InternetBanking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de transações
/// </summary>
public class TransactionRepository : Repository<Transaction>, ITransactionRepository
{
    public TransactionRepository(BankingDbContext context) : base(context)
    {
    }
    
    public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId)
    {
        return await _dbSet
            .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId)
            .Include(t => t.FromAccount)
            .Include(t => t.ToAccount)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Transaction>> GetByAccountIdAndDateRangeAsync(Guid accountId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(t => (t.FromAccountId == accountId || t.ToAccountId == accountId) &&
                       t.CreatedAt >= startDate && t.CreatedAt <= endDate)
            .Include(t => t.FromAccount)
            .Include(t => t.ToAccount)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)
            .Include(t => t.FromAccount)
            .Include(t => t.ToAccount)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }
}
