using GrowdevApi.Aplicacao.CasosDeUso.Usuario.Pegar;
using TestUtil.AutoMapper;
using TestUtil.Entidades;
using TestUtil.Servicos;

namespace UnidadeCasosDeUso.Test.Usuario.Pegar;

public class PegarUsuarioLogadoTest
{
    [Fact]
    public async Task Sucesso()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var usuarioLogado = UsuarioBuilder.Build();

        var casoDeUso = CriarCasoDeUso(cancellationToken, usuarioLogado);

        var resposta = await casoDeUso.Executar(cancellationToken);

        Assert.NotNull(resposta);
        Assert.False(string.IsNullOrWhiteSpace(resposta.Id.ToString()));
        Assert.Equal(usuarioLogado.Id.ToString(), resposta.Id);
        Assert.False(string.IsNullOrWhiteSpace(resposta.Nome));
        Assert.Equal(usuarioLogado.Nome, resposta.Nome);
        Assert.False(string.IsNullOrWhiteSpace(resposta.Email));
        Assert.Equal(usuarioLogado.Email, resposta.Email);
    }

    private static PegarUsuarioLogado CriarCasoDeUso(
        CancellationToken cancellationToken,
        GrowdevApi.Dominio.Entidades.Usuario usuarioLogado)
    {
        return new PegarUsuarioLogado(
            UsuarioLogadoServicoBuilder.Build(usuarioLogado, cancellationToken),
            MapperBuilder.Build()
        );
    }
}
