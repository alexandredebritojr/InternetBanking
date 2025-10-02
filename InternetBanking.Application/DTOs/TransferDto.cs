namespace InternetBanking.Application.DTOs;

/// <summary>
/// DTO para transferência entre contas
/// </summary>
public class TransferDto
{
    public string FromDocument { get; set; } = string.Empty;
    public string ToDocument { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
}
