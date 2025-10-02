using InternetBanking.Application.DTOs;
using InternetBanking.Application.Interfaces;
using InternetBanking.Domain.Entities;
using InternetBanking.Domain.Interfaces;

namespace InternetBanking.Application.Services;

/// <summary>
/// Serviço para operações de transação
/// </summary>
public class TransactionService : ITransactionService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAuditLogRepository _auditLogRepository;
    
    public TransactionService(
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        IAuditLogRepository auditLogRepository)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _auditLogRepository = auditLogRepository;
    }
    
    public async Task<TransferResultDto> TransferAsync(TransferDto transferDto, string userResponsible)
    {
        // Buscar contas
        var fromAccount = await _accountRepository.GetByDocumentAsync(transferDto.FromDocument);
        var toAccount = await _accountRepository.GetByDocumentAsync(transferDto.ToDocument);
        
        if (fromAccount == null)
        {
            throw new InvalidOperationException("Conta de origem não encontrada.");
        }
        
        if (toAccount == null)
        {
            throw new InvalidOperationException("Conta de destino não encontrada.");
        }
        
        // Validar se contas estão ativas
        if (fromAccount.Status != Domain.Enums.AccountStatus.Active)
        {
            throw new InvalidOperationException("Conta de origem está inativa.");
        }
        
        if (toAccount.Status != Domain.Enums.AccountStatus.Active)
        {
            throw new InvalidOperationException("Conta de destino está inativa.");
        }
        
        // Validar saldo suficiente
        if (!fromAccount.CanWithdraw(transferDto.Amount))
        {
            throw new InvalidOperationException("Saldo insuficiente para realizar a transferência.");
        }
        
        // Validar valor positivo
        if (transferDto.Amount <= 0)
        {
            throw new InvalidOperationException("Valor da transferência deve ser maior que zero.");
        }
        
        // Validar transferência para a mesma conta
        if (fromAccount.Id == toAccount.Id)
        {
            throw new InvalidOperationException("Não é possível transferir para a mesma conta.");
        }
        
        // Realizar transferência
        fromAccount.Withdraw(transferDto.Amount);
        toAccount.Deposit(transferDto.Amount);
        
        // Criar transação
        var transaction = new Transaction(
            fromAccount.Id,
            toAccount.Id,
            transferDto.Amount,
            transferDto.Description
        );
        
        // Salvar alterações
        _accountRepository.Update(fromAccount);
        _accountRepository.Update(toAccount);
        await _transactionRepository.AddAsync(transaction);
        await _transactionRepository.SaveChangesAsync();
        
        // Log de auditoria
        var auditLog = new AuditLog(
            "TRANSFER_EXECUTED",
            "Transaction",
            transaction.Id.ToString(),
            userResponsible,
            $"Transferência de R$ {transferDto.Amount:F2} de {transferDto.FromDocument} para {transferDto.ToDocument}"
        );
        await _auditLogRepository.AddAsync(auditLog);
        await _auditLogRepository.SaveChangesAsync();
        
        return new TransferResultDto
        {
            TransactionId = transaction.Id,
            FromAccount = fromAccount.Document,
            ToAccount = toAccount.Document,
            Amount = transaction.Amount,
            CreatedAt = transaction.CreatedAt,
            Description = transaction.Description
        };
    }
    
    public async Task<IEnumerable<TransferResultDto>> GetAccountTransactionsAsync(Guid accountId)
    {
        var transactions = await _transactionRepository.GetByAccountIdAsync(accountId);
        return transactions.Select(MapToDto);
    }
    
    public async Task<IEnumerable<TransferResultDto>> GetAccountTransactionsByDocumentAsync(string document)
    {
        var account = await _accountRepository.GetByDocumentAsync(document);
        if (account == null)
        {
            throw new InvalidOperationException("Conta não encontrada.");
        }
        
        return await GetAccountTransactionsAsync(account.Id);
    }
    
    private static TransferResultDto MapToDto(Transaction transaction)
    {
        return new TransferResultDto
        {
            TransactionId = transaction.Id,
            FromAccount = transaction.FromAccount?.Document ?? string.Empty,
            ToAccount = transaction.ToAccount?.Document ?? string.Empty,
            Amount = transaction.Amount,
            CreatedAt = transaction.CreatedAt,
            Description = transaction.Description
        };
    }
}