using InternetBanking.Domain.Enums;

namespace InternetBanking.Domain.Entities;

/// <summary>
/// Representa uma transação bancária
/// </summary>
public class Transaction
{
    public Guid Id { get; set; }
    public Guid FromAccountId { get; set; }
    public Guid ToAccountId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Description { get; set; } = string.Empty;
    
    // Navigation properties
    public Account FromAccount { get; set; } = null!;
    public Account ToAccount { get; set; } = null!;
    
    public Transaction()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }
    
    public Transaction(Guid fromAccountId, Guid toAccountId, decimal amount, string description = "") : this()
    {
        FromAccountId = fromAccountId;
        ToAccountId = toAccountId;
        Amount = amount;
        Type = TransactionType.Transfer;
        Description = description;
    }
}