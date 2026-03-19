using Bogus;
using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Dominio.Entidades;
using GrowdevApi.Dominio.Enums;

namespace TestUtil.Requisicoes;

public class RequisicaoAlterarUsuarioBuilder
{
    public static RequisicaoAlterarUsuario Build(string uniqueSuffix = "")
    {
        return new Faker<RequisicaoAlterarUsuario>()
            .RuleFor(requisicao => requisicao.Id, faker => faker.Random.Guid().ToString())
            .RuleFor(requisicao => requisicao.Nome, faker => faker.Person.FirstName)
            .RuleFor(requisicao => requisicao.Email, (faker, usuario) => faker.Internet.Email(usuario.Nome, uniqueSuffix: uniqueSuffix))
            .RuleFor(requisicao => requisicao.Status, fake => fake.PickRandom(EnumUtil.PegarListaNomesEnum<StatusUsuario>(["Indefinido"])));
    }

    public static RequisicaoAlterarUsuario Build(Usuario usuario)
    {
        return new RequisicaoAlterarUsuario
        {
            Id = usuario.Id.ToString(),
            Nome = usuario.Nome,
            Email = usuario.Email,
            Status = usuario.Status.ToString()
        };
    }
}
