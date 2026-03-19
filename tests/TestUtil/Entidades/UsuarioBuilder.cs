using Bogus;
using GrowdevApi.Dominio.Entidades;
using GrowdevApi.Dominio.Enums;

namespace TestUtil.Entidades;

public class UsuarioBuilder
{
    public static Usuario Build()
    {
        return new Faker<Usuario>()
            .RuleFor(usuario => usuario.Id, () => Guid.NewGuid())
            .RuleFor(usuario => usuario.Nome, fake => fake.Name.FirstName())
            .RuleFor(usuario => usuario.Email, (fake, usuario) => fake.Internet.Email(usuario.Nome))
            .RuleFor(usuario => usuario.Senha, fake => fake.Internet.Password())
            .RuleFor(usuario => usuario.Status, fake => fake.PickRandom(EnumUtil.PegarListaEnum<StatusUsuario>([StatusUsuario.Indefinido])));
    }
}
