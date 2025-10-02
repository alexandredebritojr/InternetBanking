using FluentValidation;
using System.Text.RegularExpressions;

namespace InternetBanking.Application.Validators
{
    public static class DocumentoValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> CpfOuCnpjValido<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("O documento é obrigatório.")
                .Must(IsCpfOrCnpj).WithMessage("Documento inválido. Informe um CPF ou CNPJ válido.");
        }

        private static bool IsCpfOrCnpj(string documento)
        {
            if (string.IsNullOrWhiteSpace(documento))
                return false;

            documento = Regex.Replace(documento, @"[^\d]", ""); // só números

            if (documento.Length == 11)
                return IsCpf(documento);

            if (documento.Length == 14)
                return IsCnpj(documento);

            return false;
        }

        private static bool IsCpf(string cpf)
        {
            if (cpf.Length != 11) return false;
            if (new string(cpf[0], cpf.Length) == cpf) return false;

            int[] mult1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mult2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string temp = cpf.Substring(0, 9);
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(temp[i].ToString()) * mult1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            string digito = resto.ToString();
            temp += digito;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(temp[i].ToString()) * mult2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }

        private static bool IsCnpj(string cnpj)
        {
            if (cnpj.Length != 14) return false;
            if (new string(cnpj[0], cnpj.Length) == cnpj) return false;

            int[] mult1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mult2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string temp = cnpj.Substring(0, 12);
            int soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(temp[i].ToString()) * mult1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            string digito = resto.ToString();
            temp += digito;

            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(temp[i].ToString()) * mult2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito += resto.ToString();

            return cnpj.EndsWith(digito);
        }
    }

}