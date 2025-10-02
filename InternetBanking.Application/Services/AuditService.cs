using InternetBanking.Application.DTOs;
using InternetBanking.Application.Interfaces;
using InternetBanking.Domain.Interfaces;

namespace InternetBanking.Application.Services;

/// <summary>
/// Serviço para operações de auditoria
/// </summary>
public class AuditService : IAuditService
{
    private readonly IAuditLogRepository _auditLogRepository;
    
    public AuditService(IAuditLogRepository auditLogRepository)
    {
        _auditLogRepository = auditLogRepository;
    }

    public async Task<IEnumerable<AuditLogDto>> GetAuditLogsAsync(string? entityType = null, string? entityId = null)
    {
        IEnumerable<Domain.Entities.AuditLog> logs;
        
        if (!string.IsNullOrEmpty(entityType) && !string.IsNullOrEmpty(entityId))
        {
            logs = await _auditLogRepository.GetByEntityIdAsync(entityId);
            logs = logs.Where(l => l.EntityType == entityType);
        }
        else if (!string.IsNullOrEmpty(entityType))
        {
            logs = await _auditLogRepository.GetByEntityTypeAsync(entityType);
        }
        else if (!string.IsNullOrEmpty(entityId))
        {
            logs = await _auditLogRepository.GetByEntityIdAsync(entityId);
        }
        else
        {
            logs = await _auditLogRepository.GetAllAsync();
        }
        
        return logs.Select(MapToDto);
    }
    
    private static AuditLogDto MapToDto(Domain.Entities.AuditLog auditLog)
    {
        return new AuditLogDto
        {
            Id = auditLog.Id,
            Action = auditLog.Action,
            EntityType = auditLog.EntityType,
            EntityId = auditLog.EntityId,
            UserResponsible = auditLog.UserResponsible,
            Timestamp = auditLog.Timestamp,
            Details = auditLog.Details
        };
    }
}