using InternetBanking.Application.Interfaces;
using InternetBanking.Application.Services;

namespace InternetBanking.Api.Extensions;

/// <summary>
/// Extensões para configuração de serviços da API
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// Configura os serviços de aplicação
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IAuditService, AuditService>();
        
        return services;
    }
}