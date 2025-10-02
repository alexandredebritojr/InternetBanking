using FluentAssertions;
using InternetBanking.Application.DTOs;
using System.Net.Http.Json;

namespace InternetBanking.IntegrationTests;

// Collection fixtures para isolar os testes
[CollectionDefinition("Accounts Tests")]
public class AccountsTestCollection : ICollectionFixture<CustomWebApplicationFactory> { }

[Collection("Accounts Tests")]
public class AccountsControllerIntegrationTests
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public AccountsControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateAccount_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        var createAccountDto = new CreateAccountDto
        {
            ClientName = "João Silva",
            Document = "12345678901"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/accounts", createAccountDto);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        
        var account = await response.Content.ReadFromJsonAsync<AccountDto>();
        account.Should().NotBeNull();
        account!.ClientName.Should().Be("João Silva");
        account.Document.Should().Be("12345678901");
        account.Balance.Should().Be(1000.00m);
    }

    [Fact]
    public async Task CreateAccount_WithExistingDocument_ShouldReturnConflict()
    {
        // Arrange
        var createAccountDto = new CreateAccountDto
        {
            ClientName = "João Silva",
            Document = "12345678901"
        };

        // Primeiro, cria uma conta
        await _client.PostAsJsonAsync("/api/accounts", createAccountDto);

        // Act - Tenta criar outra conta com o mesmo documento
        var response = await _client.PostAsJsonAsync("/api/accounts", createAccountDto);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task GetAccounts_ShouldReturnList()
    {
        // Arrange
        var createAccountDto = new CreateAccountDto
        {
            ClientName = "João Silva",
            Document = "12345678901"
        };

        await _client.PostAsJsonAsync("/api/accounts", createAccountDto);

        // Act
        var response = await _client.GetAsync("/api/accounts");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        
        var accounts = await response.Content.ReadFromJsonAsync<IEnumerable<AccountDto>>();
        accounts.Should().NotBeNull();
        accounts!.Should().HaveCount(1);
    }

    [Fact]
    public async Task DeactivateAccount_WithValidDocument_ShouldReturnOk()
    {
        // Arrange
        var createAccountDto = new CreateAccountDto
        {
            ClientName = "João Silva",
            Document = "12345678901"
        };

        await _client.PostAsJsonAsync("/api/accounts", createAccountDto);

        // Act
        var response = await _client.PutAsync($"/api/accounts/document/{createAccountDto.Document}/deactivate", null);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        
        var account = await response.Content.ReadFromJsonAsync<AccountDto>();
        account.Should().NotBeNull();
        account!.Status.Should().Be(InternetBanking.Domain.Enums.AccountStatus.Inactive);
    }
}
