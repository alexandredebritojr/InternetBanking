namespace InternetBanking.Application.DTOs;

/// <summary>
/// DTO para resultado de transferência
/// </summary>
public class TransferResultDto
{
    public Guid TransactionId { get; set; }
    public string FromAccount { get; set; } = string.Empty;
    public string ToAccount { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Description { get; set; } = string.Empty;
}
