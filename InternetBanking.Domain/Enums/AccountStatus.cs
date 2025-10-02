namespace InternetBanking.Domain.Enums;

/// <summary>
/// Status de uma conta bancária
/// </summary>
public enum AccountStatus
{
    /// <summary>
    /// Conta ativa - pode realizar transações
    /// </summary>
    Active = 1,
    
    /// <summary>
    /// Conta inativa - não pode realizar transações
    /// </summary>
    Inactive = 2
}