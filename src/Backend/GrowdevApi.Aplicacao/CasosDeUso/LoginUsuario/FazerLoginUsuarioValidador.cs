using FluentValidation;
using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Excecao;

namespace GrowdevApi.Aplicacao.CasosDeUso.LoginUsuario;

public class FazerLoginUsuarioValidador : AbstractValidator<RequisicaoLoginUsuario>
{
    public FazerLoginUsuarioValidador()
    {
        RuleFor(requisicao => requisicao.Email).NotEmpty().WithMessage(MensagensExcecao.EMAIL_VAZIO);
        RuleFor(requisicao => requisicao.Senha).NotEmpty().WithMessage(MensagensExcecao.SENHA_VAZIA);
    }
}
