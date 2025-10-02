using Microsoft.EntityFrameworkCore;
using InternetBanking.Api.Extensions;
using InternetBanking.Application.Interfaces;
using InternetBanking.Application.Services;
using InternetBanking.Application.Validators;
using InternetBanking.Domain.Interfaces;
using InternetBanking.Infrastructure.Data;
using InternetBanking.Infrastructure.Repositories;
using InternetBanking.Infrastructure.Extensions;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "Internet Banking API", 
        Version = "v1",
        Description = "API do Sistema de Caixa de Banco - Permite cadastro de contas e transferências entre contas bancárias",
        Contact = new() { Name = "Equipe de Desenvolvimento" }
    });
    
    // Incluir comentários XML se existirem
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Configurar infraestrutura
builder.Services.AddInfrastructureServices(builder.Configuration);

// Configurar serviços de aplicação
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IAuditService, AuditService>();

// Configurar validadores
builder.Services.AddValidatorsFromAssemblyContaining<CreateAccountValidator>();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Internet Banking API V1");
        c.RoutePrefix = "swagger"; // Para acessar Swagger em /swagger
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Aplicar migrations do banco de dados (apenas se não for ambiente de teste)
if (!app.Environment.IsEnvironment("Testing"))
{
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var context = scope.ServiceProvider.GetRequiredService<BankingDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            
            logger.LogInformation("Verificando conexão com o banco de dados...");
            
            // Aplicar migrations pendentes
            logger.LogInformation("Aplicando migrations pendentes...");
            context.Database.Migrate();
            
            logger.LogInformation("Banco de dados configurado com sucesso!");
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Erro ao configurar o banco de dados: {Message}", ex.Message);
            throw;
        }
    }
}

app.Run();

// Torna a classe Program pública para os testes de integração
public partial class Program { }
