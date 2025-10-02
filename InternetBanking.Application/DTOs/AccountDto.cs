using InternetBanking.Domain.Enums;

namespace InternetBanking.Application.DTOs;

/// <summary>
/// DTO para retorno de conta bancária
/// </summary>
public class AccountDto
{
    public Guid Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public DateTime OpeningDate { get; set; }
    public AccountStatus Status { get; set; }
}
