using InternetBanking.Application.DTOs;

namespace InternetBanking.Application.Interfaces;

/// <summary>
/// Interface para serviços de transação
/// </summary>
public interface ITransactionService
{
    Task<TransferResultDto> TransferAsync(TransferDto transferDto, string userResponsible);
    Task<IEnumerable<TransferResultDto>> GetAccountTransactionsAsync(Guid accountId);
    Task<IEnumerable<TransferResultDto>> GetAccountTransactionsByDocumentAsync(string document);
}