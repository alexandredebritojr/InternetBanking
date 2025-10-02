using FluentAssertions;
using InternetBanking.Application.DTOs;
using InternetBanking.Application.Validators;

namespace InternetBanking.UnitTests;

public class CreateAccountValidatorTests
{
    private readonly CreateAccountValidator _validator;

    public CreateAccountValidatorTests()
    {
        _validator = new CreateAccountValidator();
    }

    [Fact]
    public void Validate_ValidData_ShouldReturnSuccess()
    {
        // Arrange
        var createAccountDto = new CreateAccountDto
        {
            ClientName = "João Silva",
            Document = "12345678901"
        };

        // Act
        var result = _validator.Validate(createAccountDto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_EmptyClientName_ShouldReturnFailure()
    {
        // Arrange
        var createAccountDto = new CreateAccountDto
        {
            ClientName = "",
            Document = "12345678901"
        };

        // Act
        var result = _validator.Validate(createAccountDto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ClientName");
    }

    [Fact]
    public void Validate_EmptyDocument_ShouldReturnFailure()
    {
        // Arrange
        var createAccountDto = new CreateAccountDto
        {
            ClientName = "João Silva",
            Document = ""
        };

        // Act
        var result = _validator.Validate(createAccountDto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Document");
    }

    [Fact]
    public void Validate_InvalidDocumentFormat_ShouldReturnFailure()
    {
        // Arrange
        var createAccountDto = new CreateAccountDto
        {
            ClientName = "João Silva",
            Document = "123abc456"
        };

        // Act
        var result = _validator.Validate(createAccountDto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Document");
    }
}
