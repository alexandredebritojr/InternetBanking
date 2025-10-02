namespace InternetBanking.Application.DTOs;

/// <summary>
/// DTO para criação de conta bancária
/// </summary>
public class CreateAccountDto
{
    public string ClientName { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
}
