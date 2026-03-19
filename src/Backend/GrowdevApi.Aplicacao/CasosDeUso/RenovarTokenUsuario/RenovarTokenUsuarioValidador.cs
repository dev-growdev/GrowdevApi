using FluentValidation;
using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Excecao;

namespace GrowdevApi.Aplicacao.CasosDeUso.RenovarTokenUsuario;

public class RenovarTokenUsuarioValidador : AbstractValidator<RequisicaoNovoTokenUsuario>
{
    public RenovarTokenUsuarioValidador()
    {
        RuleFor(requisicao => requisicao.RefreshToken).NotEmpty().WithMessage(MensagensExcecao.REFRESH_TOKEN_VAZIO);
    }
}
