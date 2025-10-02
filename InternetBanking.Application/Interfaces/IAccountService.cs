using InternetBanking.Application.DTOs;

namespace InternetBanking.Application.Interfaces;

/// <summary>
/// Interface para serviços de conta bancária
/// </summary>
public interface IAccountService
{
    Task<AccountDto> CreateAccountAsync(CreateAccountDto createAccountDto);
    Task<AccountDto?> GetAccountByIdAsync(Guid id);
    Task<AccountDto?> GetAccountByDocumentAsync(string document);
    Task<IEnumerable<AccountDto>> GetAccountsAsync(AccountFilterDto? filter = null);
    Task<AccountDto> DeactivateAccountAsync(string document, string userResponsible);
}