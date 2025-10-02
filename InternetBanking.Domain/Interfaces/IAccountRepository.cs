using InternetBanking.Domain.Entities;

namespace InternetBanking.Domain.Interfaces;

/// <summary>
/// Interface específica para repositório de contas bancárias
/// </summary>
public interface IAccountRepository : IRepository<Account>
{
    Task<Account?> GetByDocumentAsync(string document);
    Task<IEnumerable<Account>> GetByStatusAsync(Enums.AccountStatus status);
    Task<IEnumerable<Account>> SearchByNameAsync(string name);
    Task<IEnumerable<Account>> SearchByDocumentAsync(string document);
    Task<bool> DocumentExistsAsync(string document);
}