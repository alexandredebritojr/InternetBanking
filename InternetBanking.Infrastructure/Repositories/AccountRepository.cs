using InternetBanking.Domain.Entities;
using InternetBanking.Domain.Enums;
using InternetBanking.Domain.Interfaces;
using InternetBanking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de contas bancárias
/// </summary>
public class AccountRepository : Repository<Account>, IAccountRepository
{
    public AccountRepository(BankingDbContext context) : base(context)
    {
    }
    
    public async Task<Account?> GetByDocumentAsync(string document)
    {
        return await _dbSet.FirstOrDefaultAsync(a => a.Document == document);
    }
    
    public async Task<IEnumerable<Account>> GetByStatusAsync(AccountStatus status)
    {
        return await _dbSet.Where(a => a.Status == status).ToListAsync();
    }
    
    public async Task<IEnumerable<Account>> SearchByNameAsync(string name)
    {
        return await _dbSet
            .Where(a => a.ClientName.Contains(name))
            .OrderBy(a => a.ClientName)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Account>> SearchByDocumentAsync(string document)
    {
        return await _dbSet
            .Where(a => a.Document.Contains(document))
            .OrderBy(a => a.Document)
            .ToListAsync();
    }
    
    public async Task<bool> DocumentExistsAsync(string document)
    {
        return await _dbSet.AnyAsync(a => a.Document == document);
    }
}