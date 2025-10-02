using InternetBanking.Domain.Enums;

namespace InternetBanking.Domain.Entities;

/// <summary>
/// Representa uma conta bancária no sistema
/// </summary>
public class Account
{
    public Guid Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public DateTime OpeningDate { get; set; }
    public AccountStatus Status { get; set; }
    
    // Navigation properties
    public ICollection<Transaction> SentTransactions { get; set; } = new List<Transaction>();
    public ICollection<Transaction> ReceivedTransactions { get; set; } = new List<Transaction>();
    
    public Account()
    {
        Id = Guid.NewGuid();
        OpeningDate = DateTime.UtcNow;
        Status = AccountStatus.Active;
        Balance = 1000.00m; // Saldo inicial de R$ 1.000,00
    }
    
    public Account(string clientName, string document) : this()
    {
        ClientName = clientName;
        Document = document;
    }
    
    public void Deactivate()
    {
        Status = AccountStatus.Inactive;
    }
    
    public bool CanWithdraw(decimal amount)
    {
        return Status == AccountStatus.Active && Balance >= amount;
    }
    
    public void Withdraw(decimal amount)
    {
        if (!CanWithdraw(amount))
            throw new InvalidOperationException("Saldo insuficiente ou conta inativa");
            
        Balance -= amount;
    }
    
    public void Deposit(decimal amount)
    {
        if (Status != AccountStatus.Active)
            throw new InvalidOperationException("Conta inativa");
            
        Balance += amount;
    }
    
    /// <summary>
    /// Obtém todas as transações da conta (enviadas e recebidas)
    /// </summary>
    public IEnumerable<Transaction> GetAllTransactions()
    {
        return SentTransactions.Concat(ReceivedTransactions)
            .OrderByDescending(t => t.CreatedAt);
    }
}
