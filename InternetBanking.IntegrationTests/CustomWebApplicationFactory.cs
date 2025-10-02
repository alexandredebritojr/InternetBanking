using InternetBanking.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InternetBanking.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private SqliteConnection? _connection;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        
        builder.ConfigureServices(services =>
        {
            // Remove o DbContext atual
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<BankingDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            // Criar conexão SQLite em memória com nome único
            var databaseName = $"TestDb_{Guid.NewGuid():N}";
            _connection = new SqliteConnection($"Data Source={databaseName};Mode=Memory;Cache=Shared");
            _connection.Open();

            // Adiciona o DbContext para testes
            services.AddDbContext<BankingDbContext>(options =>
            {
                options.UseSqlite(_connection);
                options.EnableSensitiveDataLogging();
            });
        });
        
        // Configura o host para não executar migrations
        builder.ConfigureServices(services =>
        {
            // Garante que o banco seja criado após todos os serviços serem configurados
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BankingDbContext>();
            
            // Use EnsureCreated ao invés de Migrate para testes
            context.Database.EnsureCreated();
        });
    }

    public void ResetDatabase()
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BankingDbContext>();
        
        // Limpa todas as tabelas
        context.Database.ExecuteSqlRaw("DELETE FROM Transactions");
        context.Database.ExecuteSqlRaw("DELETE FROM AuditLogs");
        context.Database.ExecuteSqlRaw("DELETE FROM Accounts");
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _connection?.Close();
            _connection?.Dispose();
        }
        base.Dispose(disposing);
    }
}
