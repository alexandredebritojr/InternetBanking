using FluentValidation;
using InternetBanking.Application.DTOs;
using InternetBanking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Api.Controllers;

/// <summary>
/// Controller para operações de transferência
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TransfersController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly IValidator<TransferDto> _transferValidator;
    
    public TransfersController(
        ITransactionService transactionService,
        IValidator<TransferDto> transferValidator)
    {
        _transactionService = transactionService;
        _transferValidator = transferValidator;
    }
    
    /// <summary>
    /// Realiza uma transferência entre contas bancárias
    /// </summary>
    /// <param name="transferDto">Dados da transferência</param>
    /// <returns>Resultado da transferência</returns>
    [HttpPost]
    [ProducesResponseType(typeof(TransferResultDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Transfer([FromBody] TransferDto transferDto)
    {
        var validationResult = await _transferValidator.ValidateAsync(transferDto);
        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
        }
        
        // Para este exemplo, vamos usar "SYSTEM" como usuário responsável
        // Em um sistema real, isso viria da autenticação/autorização
        const string userResponsible = "SYSTEM";
        
        try
        {
            var result = await _transactionService.TransferAsync(transferDto, userResponsible);
            return CreatedAtAction(nameof(GetAccountTransactions), new { document = transferDto.FromDocument }, result);
        }
        catch (InvalidOperationException ex)
        {
            var statusCode = ex.Message.Contains("não encontrada") ? StatusCodes.Status404NotFound : StatusCodes.Status400BadRequest;
            
            return StatusCode(statusCode, new ProblemDetails
            {
                Title = "Erro na transferência",
                Detail = ex.Message,
                Status = statusCode
            });
        }
    }
    
    /// <summary>
    /// Lista transações de uma conta por documento
    /// </summary>
    /// <param name="document">Documento da conta</param>
    /// <returns>Lista de transações da conta</returns>
    [HttpGet("account/{document}")]
    [ProducesResponseType(typeof(IEnumerable<TransferResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccountTransactions(string document)
    {
        try
        {
            var transactions = await _transactionService.GetAccountTransactionsByDocumentAsync(document);
            return Ok(transactions);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Conta não encontrada",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
    }
}