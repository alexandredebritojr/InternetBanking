using InternetBanking.Application.DTOs;
using InternetBanking.Application.Interfaces;
using InternetBanking.Domain.Entities;
using InternetBanking.Domain.Enums;
using InternetBanking.Domain.Interfaces;

namespace InternetBanking.Application.Services;

/// <summary>
/// Serviço para operações de conta bancária
/// </summary>
public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAuditLogRepository _auditLogRepository;
    
    public AccountService(IAccountRepository accountRepository, IAuditLogRepository auditLogRepository)
    {
        _accountRepository = accountRepository;
        _auditLogRepository = auditLogRepository;
    }
    
    public async Task<AccountDto> CreateAccountAsync(CreateAccountDto createAccountDto)
    {
        // Verificar se já existe conta com o documento
        if (await _accountRepository.DocumentExistsAsync(createAccountDto.Document))
        {
            throw new InvalidOperationException("Já existe uma conta cadastrada para este documento.");
        }
        
        // Criar nova conta
        var account = new Account(createAccountDto.ClientName, createAccountDto.Document);
        
        await _accountRepository.AddAsync(account);
        await _accountRepository.SaveChangesAsync();
        
        // Log de auditoria
        var auditLog = new AuditLog(
            "ACCOUNT_CREATED",
            "Account",
            account.Id.ToString(),
            "SYSTEM",
            $"Conta criada para {account.ClientName} - Documento: {account.Document}"
        );
        await _auditLogRepository.AddAsync(auditLog);
        await _auditLogRepository.SaveChangesAsync();
        
        return MapToDto(account);
    }
    
    public async Task<AccountDto?> GetAccountByIdAsync(Guid id)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        return account != null ? MapToDto(account) : null;
    }
    
    public async Task<AccountDto?> GetAccountByDocumentAsync(string document)
    {
        var account = await _accountRepository.GetByDocumentAsync(document);
        return account != null ? MapToDto(account) : null;
    }
    
    public async Task<IEnumerable<AccountDto>> GetAccountsAsync(AccountFilterDto? filter = null)
    {
        IEnumerable<Account> accounts;
        
        if (filter == null)
        {
            accounts = await _accountRepository.GetAllAsync();
        }
        else
        {
            // Aplicar filtros
            if (!string.IsNullOrEmpty(filter.Name))
            {
                accounts = await _accountRepository.SearchByNameAsync(filter.Name);
            }
            else if (!string.IsNullOrEmpty(filter.Document))
            {
                accounts = await _accountRepository.SearchByDocumentAsync(filter.Document);
            }
            else if (filter.Status.HasValue)
            {
                accounts = await _accountRepository.GetByStatusAsync(filter.Status.Value);
            }
            else
            {
                accounts = await _accountRepository.GetAllAsync();
            }
        }
        
        return accounts.Select(MapToDto);
    }
    
    public async Task<AccountDto> DeactivateAccountAsync(string document, string userResponsible)
    {
        var account = await _accountRepository.GetByDocumentAsync(document);
        if (account == null)
        {
            throw new InvalidOperationException("Conta não encontrada.");
        }
        
        if (account.Status == AccountStatus.Inactive)
        {
            throw new InvalidOperationException("Conta já está inativa.");
        }
        
        account.Deactivate();
        _accountRepository.Update(account);
        await _accountRepository.SaveChangesAsync();
        
        // Log de auditoria
        var auditLog = new AuditLog(
            "ACCOUNT_DEACTIVATED",
            "Account",
            account.Id.ToString(),
            userResponsible,
            $"Conta desativada - Documento: {account.Document}, Cliente: {account.ClientName}"
        );
        await _auditLogRepository.AddAsync(auditLog);
        await _auditLogRepository.SaveChangesAsync();
        
        return MapToDto(account);
    }

    private static AccountDto MapToDto(Account account)
    {
        return new AccountDto
        {
            Id = account.Id,
            ClientName = account.ClientName,
            Document = account.Document,
            Balance = account.Balance,
            OpeningDate = account.OpeningDate,
            Status = account.Status
        };
    }
}