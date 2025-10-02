using InternetBanking.Application.DTOs;

namespace InternetBanking.Application.Interfaces;

/// <summary>
/// Interface para serviços de auditoria
/// </summary>
public interface IAuditService
{
    Task<IEnumerable<AuditLogDto>> GetAuditLogsAsync(string? entityType = null, string? entityId = null);
}