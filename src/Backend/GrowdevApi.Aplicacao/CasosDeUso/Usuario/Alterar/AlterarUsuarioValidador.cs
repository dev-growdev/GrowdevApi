using FluentValidation;
using GrowdevApi.Aplicacao.Validadores;
using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Dominio.Enums;
using GrowdevApi.Excecao;

namespace GrowdevApi.Aplicacao.CasosDeUso.Usuario.Alterar;

public class AlterarUsuarioValidador : AbstractValidator<RequisicaoAlterarUsuario>
{
    public AlterarUsuarioValidador()
    {
        RuleFor(requisicao => requisicao.Id).NotEmpty().WithMessage(MensagensExcecao.ID_VAZIO);
        When(requisicao => !string.IsNullOrWhiteSpace(requisicao.Id), () =>
        {
            RuleFor(requisicao => requisicao.Id).Must(IdValidador.IdEstaValido).WithMessage(MensagensExcecao.ID_INVALIDO);
        });
        RuleFor(requisicao => requisicao.Nome).NotEmpty().WithMessage(MensagensExcecao.NOME_VAZIO);
        RuleFor(requisicao => requisicao.Email).NotEmpty().WithMessage(MensagensExcecao.EMAIL_VAZIO);
        When(requisicao => !string.IsNullOrWhiteSpace(requisicao.Email), () =>
        {
            RuleFor(requisicao => requisicao.Email).EmailAddress().WithMessage(MensagensExcecao.EMAIL_INVALIDO);
        });
        RuleFor(requisicao => requisicao.Status).NotEmpty().WithMessage(MensagensExcecao.STATUS_VAZIO);
        When(requisicao => !string.IsNullOrWhiteSpace(requisicao.Status), () =>
        {
            RuleFor(requisicao => requisicao.Status)
                .Must(status => EnumUtil.PegarListaNomesEnum<StatusUsuario>(["Indefinido"]).Contains(status!))
                .WithMessage(MensagensExcecao.STATUS_INVALIDO.Replace("{ValoresPossiveis}", EnumUtil.PegarNomesEnumSeparadosPorVirgula<StatusUsuario>(["Indefinido"])));
        });
    }
}