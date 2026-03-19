using Bogus;
using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Dominio.Entidades;

namespace TestUtil.Requisicoes;

public class RequisicaoLoginUsuarioBuilder
{
    public static RequisicaoLoginUsuario Build(int tamanhoSenha = 10)
    {
        return new Faker<RequisicaoLoginUsuario>("pt_BR")
            .RuleFor(requisicao => requisicao.Email, (faker, usuario) => faker.Internet.Email())
            .RuleFor(requisicao => requisicao.Senha, fake => fake.Internet.Password(tamanhoSenha));
    }

    public static RequisicaoLoginUsuario Build(Usuario usuario)
    {
        var senha = usuario.Senha;

        if (string.IsNullOrWhiteSpace(senha))
            senha = new Faker().Internet.Password();

        return new RequisicaoLoginUsuario
        {
            Email = usuario.Email,
            Senha = senha
        };
    }
}
