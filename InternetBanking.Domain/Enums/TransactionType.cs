namespace InternetBanking.Domain.Enums;

/// <summary>
/// Tipos de transação bancária
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Transferência entre contas
    /// </summary>
    Transfer = 1,
    
    /// <summary>
    /// Depósito
    /// </summary>
    Deposit = 2,
    
    /// <summary>
    /// Saque
    /// </summary>
    Withdraw = 3,
    
    /// <summary>
    /// Saldo inicial (bonificação)
    /// </summary>
    InitialBalance = 4
}