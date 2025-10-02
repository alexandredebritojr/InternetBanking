using InternetBanking.Application.DTOs;
using InternetBanking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Api.Controllers;

/// <summary>
/// Controller para operações de auditoria
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuditController : ControllerBase
{
    private readonly IAuditService _auditService;
    
    public AuditController(IAuditService auditService)
    {
        _auditService = auditService;
    }
    
    /// <summary>
    /// Lista logs de auditoria com filtros opcionais
    /// </summary>
    /// <param name="entityType">Filtro por tipo de entidade (Account, Transaction, etc.)</param>
    /// <param name="entityId">Filtro por ID da entidade</param>
    /// <returns>Lista de logs de auditoria</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AuditLogDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAuditLogs(
        [FromQuery] string? entityType = null,
        [FromQuery] string? entityId = null)
    {
        var logs = await _auditService.GetAuditLogsAsync(entityType, entityId);
        return Ok(logs);
    }
    
    /// <summary>
    /// Lista logs de auditoria por tipo de entidade
    /// </summary>
    /// <param name="entityType">Tipo de entidade</param>
    /// <returns>Lista de logs de auditoria</returns>
    [HttpGet("entity-type/{entityType}")]
    [ProducesResponseType(typeof(IEnumerable<AuditLogDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAuditLogsByEntityType(string entityType)
    {
        var logs = await _auditService.GetAuditLogsAsync(entityType, null);
        return Ok(logs);
    }
    
    /// <summary>
    /// Lista logs de auditoria por ID da entidade
    /// </summary>
    /// <param name="entityId">ID da entidade</param>
    /// <returns>Lista de logs de auditoria</returns>
    [HttpGet("entity-id/{entityId}")]
    [ProducesResponseType(typeof(IEnumerable<AuditLogDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAuditLogsByEntityId(string entityId)
    {
        var logs = await _auditService.GetAuditLogsAsync(null, entityId);
        return Ok(logs);
    }
}