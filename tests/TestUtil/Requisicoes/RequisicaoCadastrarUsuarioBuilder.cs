using Bogus;
using GrowdevApi.Comunicacao.Requisicoes;

namespace TestUtil.Requisicoes;

public class RequisicaoCadastrarUsuarioBuilder
{
    public static RequisicaoCadastrarUsuario Build()
    {
        return new Faker<RequisicaoCadastrarUsuario>("pt_BR")
            .RuleFor(requisicao => requisicao.Nome, faker => faker.Person.FirstName)
            .RuleFor(requisicao => requisicao.Email, (faker, usuario) => faker.Internet.Email(usuario.Nome))
            .RuleFor(requisicao => requisicao.Senha, _ => "Senha.Valida1");
    }

    public static RequisicaoCadastrarUsuario Build(GrowdevApi.Dominio.Entidades.Usuario usuario)
    {
        return new RequisicaoCadastrarUsuario
        {
            Nome = usuario.Nome,
            Email = usuario.Email,
            Senha = "Senha.Valida1"
        };
    }
}
