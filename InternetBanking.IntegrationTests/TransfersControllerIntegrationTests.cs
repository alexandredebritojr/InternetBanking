using FluentAssertions;
using InternetBanking.Application.DTOs;
using System.Net.Http.Json;

namespace InternetBanking.IntegrationTests;

[CollectionDefinition("Transfers Tests")]
public class TransfersTestCollection : ICollectionFixture<CustomWebApplicationFactory> { }

[Collection("Transfers Tests")]
public class TransfersControllerIntegrationTests
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public TransfersControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Transfer_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        var fromAccount = new CreateAccountDto
        {
            ClientName = "João Silva",
            Document = "12345678901"
        };

        var toAccount = new CreateAccountDto
        {
            ClientName = "Maria Santos",
            Document = "98765432100"
        };

        await _client.PostAsJsonAsync("/api/accounts", fromAccount);
        await _client.PostAsJsonAsync("/api/accounts", toAccount);

        var transferDto = new TransferDto
        {
            FromDocument = fromAccount.Document,
            ToDocument = toAccount.Document,
            Amount = 500.00m,
            Description = "Transferência de teste"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/transfers", transferDto);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        
        var result = await response.Content.ReadFromJsonAsync<TransferResultDto>();
        result.Should().NotBeNull();
        result!.Amount.Should().Be(500.00m);
        result.FromAccount.Should().Be(fromAccount.Document);
        result.ToAccount.Should().Be(toAccount.Document);
    }

    [Fact]
    public async Task Transfer_WithInsufficientBalance_ShouldReturnBadRequest()
    {
        // Arrange
        var fromAccount = new CreateAccountDto
        {
            ClientName = "João Silva",
            Document = "12345678901"
        };

        var toAccount = new CreateAccountDto
        {
            ClientName = "Maria Santos",
            Document = "98765432100"
        };

        await _client.PostAsJsonAsync("/api/accounts", fromAccount);
        await _client.PostAsJsonAsync("/api/accounts", toAccount);

        var transferDto = new TransferDto
        {
            FromDocument = fromAccount.Document,
            ToDocument = toAccount.Document,
            Amount = 1500.00m, // Mais que o saldo inicial
            Description = "Transferência com saldo insuficiente"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/transfers", transferDto);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}