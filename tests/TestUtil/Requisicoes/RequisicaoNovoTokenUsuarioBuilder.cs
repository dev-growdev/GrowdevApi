using Bogus;
using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Dominio.Entidades;
using TestUtil.Extensoes;

namespace TestUtil.Requisicoes;

public class RequisicaoNovoTokenUsuarioBuilder
{
    public static RequisicaoNovoTokenUsuario Build()
    {
        return new Faker<RequisicaoNovoTokenUsuario>("pt_BR")
            .RuleFor(requisicao => requisicao.RefreshToken, faker => faker.Random.AlphaNumericUrl(24));
    }

    public static RequisicaoNovoTokenUsuario Build(RefreshToken refreshToken)
    {
        return new RequisicaoNovoTokenUsuario
        {
            RefreshToken = refreshToken.Valor
        };
    }
}
