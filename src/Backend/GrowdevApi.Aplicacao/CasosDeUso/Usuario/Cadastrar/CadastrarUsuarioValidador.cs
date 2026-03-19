using FluentValidation;
using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Excecao;
using System.Text.RegularExpressions;

namespace GrowdevApi.Aplicacao.CasosDeUso.Usuario.Cadastrar;

public class CadastrarUsuarioValidador : AbstractValidator<RequisicaoCadastrarUsuario>
{
    public CadastrarUsuarioValidador()
    {
        RuleFor(requisicao => requisicao.Nome).NotEmpty().WithMessage(MensagensExcecao.NOME_VAZIO);
        RuleFor(requisicao => requisicao.Email).NotEmpty().WithMessage(MensagensExcecao.EMAIL_VAZIO);
        When(requisicao => !string.IsNullOrWhiteSpace(requisicao.Email), () =>
        {
            RuleFor(requisicao => requisicao.Email).EmailAddress().WithMessage(MensagensExcecao.EMAIL_INVALIDO);
        });
        RuleFor(requisicao => requisicao.Senha).NotEmpty().WithMessage(MensagensExcecao.SENHA_VAZIA);
        When(requisicao => !string.IsNullOrWhiteSpace(requisicao.Senha), () =>
        {
            RuleFor(requisicao => requisicao.Senha).Must(SenhaValida!).WithMessage(MensagensExcecao.SENHA_INVALIDA);
        });
    }

    private bool SenhaValida(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha)
            || senha.Length < 8
            || !Regex.IsMatch(senha, @"[A-Z]")
            || !Regex.IsMatch(senha, @"[a-z]")
            || !Regex.IsMatch(senha, @"[\d]")
            || !Regex.IsMatch(senha, @"[\W_]"))
        {
            return false;
        }

        return true;
    }
}
