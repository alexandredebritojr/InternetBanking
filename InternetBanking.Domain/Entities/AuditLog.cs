namespace InternetBanking.Domain.Entities;

/// <summary>
/// Representa um log de auditoria para ações importantes do sistema
/// </summary>
public class AuditLog
{
    public Guid Id { get; set; }
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string UserResponsible { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Details { get; set; } = string.Empty;
    
    public AuditLog()
    {
        Id = Guid.NewGuid();
        Timestamp = DateTime.UtcNow;
    }
    
    public AuditLog(string action, string entityType, string entityId, string userResponsible, string details = "") : this()
    {
        Action = action;
        EntityType = entityType;
        EntityId = entityId;
        UserResponsible = userResponsible;
        Details = details;
    }
}