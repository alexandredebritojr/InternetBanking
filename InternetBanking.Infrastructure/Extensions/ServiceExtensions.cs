using InternetBanking.Domain.Interfaces;
using InternetBanking.Infrastructure.Data;
using InternetBanking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InternetBanking.Infrastructure.Extensions;

/// <summary>
/// Extensões para configuração de serviços da camada de infraestrutura
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// Configura os serviços de banco de dados
    /// </summary>
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuração do banco de dados SQL Server
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }
        
        services.AddDbContext<BankingDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            }));
        
        return services;
    }
    
    /// <summary>
    /// Configura os repositórios
    /// </summary>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        
        return services;
    }
    
    /// <summary>
    /// Configura todos os serviços da infraestrutura
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseServices(configuration);
        services.AddRepositories();
        
        return services;
    }
}