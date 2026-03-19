using Bogus;
using GrowdevApi.Dominio.Entidades;
using TestUtil.Extensoes;

namespace TestUtil.Entidades;

public class RefreshTokenBuilder
{
    public static RefreshToken Build(Usuario? usuario = null)
    {
        usuario ??= UsuarioBuilder.Build();

        return new Faker<RefreshToken>("pt_BR")
            .RuleFor(requisicao => requisicao.Id, () => Guid.NewGuid())
            .RuleFor(requisicao => requisicao.Valor, faker => faker.Random.AlphaNumericUrl(24))
            .RuleFor(tokenAtivacao => tokenAtivacao.IdUsuario, () => usuario.Id)
            .RuleFor(tokenAtivacao => tokenAtivacao.Usuario, () => usuario);
    }
}
