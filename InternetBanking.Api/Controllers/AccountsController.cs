using FluentValidation;
using InternetBanking.Application.DTOs;
using InternetBanking.Application.Interfaces;
using InternetBanking.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Api.Controllers;

/// <summary>
/// Controller para operações de contas bancárias
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IValidator<CreateAccountDto> _createAccountValidator;
    
    public AccountsController(
        IAccountService accountService,
        IValidator<CreateAccountDto> createAccountValidator)
    {
        _accountService = accountService;
        _createAccountValidator = createAccountValidator;
    }
    
    /// <summary>
    /// Cadastra uma nova conta bancária
    /// </summary>
    /// <param name="createAccountDto">Dados da conta a ser criada</param>
    /// <returns>Dados da conta criada</returns>
    [HttpPost]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto createAccountDto)
    {
        var validationResult = await _createAccountValidator.ValidateAsync(createAccountDto);
        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
        }
        
        try
        {
            var account = await _accountService.CreateAccountAsync(createAccountDto);
            return CreatedAtAction(nameof(GetAccountByDocument), new { document = account.Document }, account);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Erro ao criar conta",
                Detail = ex.Message,
                Status = StatusCodes.Status409Conflict
            });
        }
    }
    
    /// <summary>
    /// Lista contas bancárias com filtros opcionais
    /// </summary>
    /// <param name="name">Filtro por nome do cliente (parcial)</param>
    /// <param name="document">Filtro por documento (parcial)</param>
    /// <param name="status">Filtro por status da conta</param>
    /// <returns>Lista de contas</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AccountDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAccounts(
        [FromQuery] string? name = null,
        [FromQuery] string? document = null,
        [FromQuery] AccountStatus? status = null)
    {
        var filter = new AccountFilterDto
        {
            Name = name,
            Document = document,
            Status = status
        };
        
        var accounts = await _accountService.GetAccountsAsync(filter);
        return Ok(accounts);
    }
    
    /// <summary>
    /// Busca uma conta pelo documento
    /// </summary>
    /// <param name="document">Documento da conta</param>
    /// <returns>Dados da conta</returns>
    [HttpGet("document/{document}")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccountByDocument(string document)
    {
        var account = await _accountService.GetAccountByDocumentAsync(document);
        if (account == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Conta não encontrada",
                Detail = $"Nenhuma conta encontrada para o documento: {document}",
                Status = StatusCodes.Status404NotFound
            });
        }
        
        return Ok(account);
    }
    
    /// <summary>
    /// Busca uma conta pelo ID
    /// </summary>
    /// <param name="id">ID da conta</param>
    /// <returns>Dados da conta</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccountById(Guid id)
    {
        var account = await _accountService.GetAccountByIdAsync(id);
        if (account == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Conta não encontrada",
                Detail = $"Nenhuma conta encontrada para o ID: {id}",
                Status = StatusCodes.Status404NotFound
            });
        }
        
        return Ok(account);
    }
    
    /// <summary>
    /// Desativa uma conta bancária
    /// </summary>
    /// <param name="document">Documento da conta a ser desativada</param>
    /// <returns>Dados da conta desativada</returns>
    [HttpPut("document/{document}/deactivate")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeactivateAccount(string document)
    {
        // Para este exemplo, vamos usar "SYSTEM" como usuário responsável
        // Em um sistema real, isso viria da autenticação/autorização
        const string userResponsible = "SYSTEM";
        
        try
        {
            var account = await _accountService.DeactivateAccountAsync(document, userResponsible);
            return Ok(account);
        }
        catch (InvalidOperationException ex)
        {
            var statusCode = ex.Message.Contains("não encontrada") ? StatusCodes.Status404NotFound : StatusCodes.Status400BadRequest;
            
            return StatusCode(statusCode, new ProblemDetails
            {
                Title = "Erro ao desativar conta",
                Detail = ex.Message,
                Status = statusCode
            });
        }
    }
}