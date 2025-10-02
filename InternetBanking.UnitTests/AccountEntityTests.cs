using FluentAssertions;
using InternetBanking.Domain.Entities;
using InternetBanking.Domain.Enums;

namespace InternetBanking.UnitTests;

public class AccountEntityTests
{
    [Fact]
    public void Constructor_ShouldSetInitialValues()
    {
        // Act
        var account = new Account("João Silva", "12345678901");

        // Assert
        account.Id.Should().NotBeEmpty();
        account.ClientName.Should().Be("João Silva");
        account.Document.Should().Be("12345678901");
        account.Balance.Should().Be(1000.00m);
        account.Status.Should().Be(AccountStatus.Active);
        account.OpeningDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Deactivate_ShouldChangeStatusToInactive()
    {
        // Arrange
        var account = new Account("João Silva", "12345678901");

        // Act
        account.Deactivate();

        // Assert
        account.Status.Should().Be(AccountStatus.Inactive);
    }

    [Fact]
    public void CanWithdraw_WithSufficientBalance_ShouldReturnTrue()
    {
        // Arrange
        var account = new Account("João Silva", "12345678901");

        // Act
        var result = account.CanWithdraw(500m);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CanWithdraw_WithInsufficientBalance_ShouldReturnFalse()
    {
        // Arrange
        var account = new Account("João Silva", "12345678901");

        // Act
        var result = account.CanWithdraw(1500m);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void CanWithdraw_WithInactiveAccount_ShouldReturnFalse()
    {
        // Arrange
        var account = new Account("João Silva", "12345678901");
        account.Deactivate();

        // Act
        var result = account.CanWithdraw(500m);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Withdraw_WithValidAmount_ShouldDecreaseBalance()
    {
        // Arrange
        var account = new Account("João Silva", "12345678901");
        var initialBalance = account.Balance;

        // Act
        account.Withdraw(300m);

        // Assert
        account.Balance.Should().Be(initialBalance - 300m);
    }

    [Fact]
    public void Withdraw_WithInsufficientBalance_ShouldThrowException()
    {
        // Arrange
        var account = new Account("João Silva", "12345678901");

        // Act & Assert
        var action = () => account.Withdraw(1500m);
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Saldo insuficiente ou conta inativa");
    }

    [Fact]
    public void Deposit_WithValidAmount_ShouldIncreaseBalance()
    {
        // Arrange
        var account = new Account("João Silva", "12345678901");
        var initialBalance = account.Balance;

        // Act
        account.Deposit(500m);

        // Assert
        account.Balance.Should().Be(initialBalance + 500m);
    }

    [Fact]
    public void Deposit_WithInactiveAccount_ShouldThrowException()
    {
        // Arrange
        var account = new Account("João Silva", "12345678901");
        account.Deactivate();

        // Act & Assert
        var action = () => account.Deposit(500m);
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Conta inativa");
    }
}
