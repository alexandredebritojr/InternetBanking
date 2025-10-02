using FluentValidation;
using InternetBanking.Application.DTOs;

namespace InternetBanking.Application.Validators;

/// <summary>
/// Validador para criação de conta bancária
/// </summary>
public class CreateAccountValidator : AbstractValidator<CreateAccountDto>
{
    public CreateAccountValidator()
    {
        RuleFor(x => x.ClientName)
            .NotEmpty()
            .WithMessage("Nome do cliente é obrigatório")
            .MaximumLength(200)
            .WithMessage("Nome do cliente não pode exceder 200 caracteres");

        RuleFor(f => f.Document).CpfOuCnpjValido();
    }
}