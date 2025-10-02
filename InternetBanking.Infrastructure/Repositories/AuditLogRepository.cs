using InternetBanking.Domain.Entities;
using InternetBanking.Domain.Interfaces;
using InternetBanking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de logs de auditoria
/// </summary>
public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
{
    public AuditLogRepository(BankingDbContext context) : base(context)
    {
    }
    
    public async Task<IEnumerable<AuditLog>> GetByEntityTypeAsync(string entityType)
    {
        return await _dbSet
            .Where(a => a.EntityType == entityType)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<AuditLog>> GetByEntityIdAsync(string entityId)
    {
        return await _dbSet
            .Where(a => a.EntityId == entityId)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(a => a.Timestamp >= startDate && a.Timestamp <= endDate)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<AuditLog>> GetByUserAsync(string userResponsible)
    {
        return await _dbSet
            .Where(a => a.UserResponsible == userResponsible)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();
    }
}
