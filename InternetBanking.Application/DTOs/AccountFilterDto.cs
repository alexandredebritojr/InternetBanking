using InternetBanking.Domain.Enums;

namespace InternetBanking.Application.DTOs;

/// <summary>
/// DTO para filtro de contas
/// </summary>
public class AccountFilterDto
{
    public string? Name { get; set; }
    public string? Document { get; set; }
    public AccountStatus? Status { get; set; }
}
