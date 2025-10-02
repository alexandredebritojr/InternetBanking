using InternetBanking.Domain.Entities;

namespace InternetBanking.Domain.Interfaces;

/// <summary>
/// Interface específica para repositório de logs de auditoria
/// </summary>
public interface IAuditLogRepository : IRepository<AuditLog>
{
    Task<IEnumerable<AuditLog>> GetByEntityTypeAsync(string entityType);
    Task<IEnumerable<AuditLog>> GetByEntityIdAsync(string entityId);
}