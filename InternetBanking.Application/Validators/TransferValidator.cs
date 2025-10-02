using FluentValidation;
using InternetBanking.Application.DTOs;

namespace InternetBanking.Application.Validators;

/// <summary>
/// Validador para transferência entre contas
/// </summary>
public class TransferValidator : AbstractValidator<TransferDto>
{
    public TransferValidator()
    {
        RuleFor(f => f.FromDocument).CpfOuCnpjValido();

        RuleFor(f => f.ToDocument).CpfOuCnpjValido();

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Valor da transferência deve ser maior que zero")
            .LessThanOrEqualTo(1000000)
            .WithMessage("Valor da transferência não pode exceder R$ 1.000.000,00");
            
        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Descrição não pode exceder 500 caracteres");
    }
}