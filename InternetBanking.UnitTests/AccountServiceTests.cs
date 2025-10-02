using FluentAssertions;
using InternetBanking.Application.DTOs;
using InternetBanking.Application.Services;
using InternetBanking.Domain.Entities;
using InternetBanking.Domain.Enums;
using InternetBanking.Domain.Interfaces;
using Moq;

namespace InternetBanking.UnitTests;

public class AccountServiceTests
{
    private readonly Mock<IAccountRepository> _mockAccountRepository;
    private readonly Mock<IAuditLogRepository> _mockAuditLogRepository;
    private readonly AccountService _accountService;

    public AccountServiceTests()
    {
        _mockAccountRepository = new Mock<IAccountRepository>();
        _mockAuditLogRepository = new Mock<IAuditLogRepository>();
        _accountService = new AccountService(_mockAccountRepository.Object, _mockAuditLogRepository.Object);
    }

    [Fact]
    public async Task CreateAccountAsync_WithValidData_ShouldCreateAccount()
    {
        // Arrange
        var createAccountDto = new CreateAccountDto
        {
            ClientName = "João Silva",
            Document = "12345678901"
        };

        _mockAccountRepository.Setup(x => x.DocumentExistsAsync(createAccountDto.Document))
            .ReturnsAsync(false);
        _mockAccountRepository.Setup(x => x.AddAsync(It.IsAny<Account>()))
            .ReturnsAsync((Account a) => a);
        _mockAccountRepository.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);
        _mockAuditLogRepository.Setup(x => x.AddAsync(It.IsAny<AuditLog>()))
            .ReturnsAsync((AuditLog a) => a);
        _mockAuditLogRepository.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _accountService.CreateAccountAsync(createAccountDto);

        // Assert
        result.Should().NotBeNull();
        result.ClientName.Should().Be("João Silva");
        result.Document.Should().Be("12345678901");
        result.Balance.Should().Be(1000.00m);
        result.Status.Should().Be(AccountStatus.Active);
    }

    [Fact]
    public async Task CreateAccountAsync_WithExistingDocument_ShouldThrowException()
    {
        // Arrange
        var createAccountDto = new CreateAccountDto
        {
            ClientName = "João Silva",
            Document = "12345678901"
        };

        _mockAccountRepository.Setup(x => x.DocumentExistsAsync(createAccountDto.Document))
            .ReturnsAsync(true);

        // Act & Assert
        var action = async () => await _accountService.CreateAccountAsync(createAccountDto);
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Já existe uma conta cadastrada para este documento.");
    }
}