using GrowdevApi.Aplicacao.CasosDeUso.Usuario.Pegar;
using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Excecao;
using TestUtil.AutoMapper;
using TestUtil.Entidades;
using TestUtil.Repositorios;

namespace UnidadeCasosDeUso.Test.Usuario.Pegar;

public class PegarUsuarioPorIdTest
{
    [Fact]
    public async Task Sucesso()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var usuarioLogado = UsuarioBuilder.Build();
        var usuarioPegar = UsuarioBuilder.Build();

        var casoDeUso = CriarCasoDeUso(cancellationToken, usuarioLogado, usuarioPegar);

        var requisicao = new RequisicaoPegarUsuario()
        {
            Id = usuarioPegar.Id.ToString()
        };

        var resposta = await casoDeUso.Executar(requisicao, cancellationToken);

        Assert.NotNull(resposta);
        Assert.False(string.IsNullOrWhiteSpace(resposta.Id.ToString()));
        Assert.Equal(usuarioPegar.Id.ToString(), resposta.Id);
        Assert.False(string.IsNullOrWhiteSpace(resposta.Nome));
        Assert.Equal(usuarioPegar.Nome, resposta.Nome);
        Assert.False(string.IsNullOrWhiteSpace(resposta.Email));
        Assert.Equal(usuarioPegar.Email, resposta.Email);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("asdfasdf")]
    [InlineData("b9fc55af-e38u-4852-b4f5-ad6b1277472d")]
    public async Task Erro_Id_Invalido(string id)
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var usuarioLogado = UsuarioBuilder.Build();

        var requisicao = new RequisicaoPegarUsuario()
        {
            Id = id,
        };

        var casoDeUso = CriarCasoDeUso(cancellationToken: cancellationToken, usuarioLogado: usuarioLogado, usuarioPegar: usuarioLogado);

        var ex = await Assert.ThrowsAsync<ErroValidacaoExcecao>(async () => await casoDeUso.Executar(requisicao, cancellationToken));
        Assert.NotNull(ex);
        var mensagensDeErro = ex.PegarMensagensDeErro();
        Assert.Single(mensagensDeErro);
        Assert.Contains(MensagensExcecao.ID_INVALIDO, mensagensDeErro);
    }

    [Fact]
    public async Task Erro_Usuario_Nao_Encontrado()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var usuarioLogado = UsuarioBuilder.Build();

        var requisicao = new RequisicaoPegarUsuario()
        {
            Id = Guid.NewGuid().ToString(),
        };

        var casoDeUso = CriarCasoDeUso(cancellationToken: cancellationToken, usuarioLogado: usuarioLogado, usuarioPegar: usuarioLogado);

        var ex = await Assert.ThrowsAsync<NaoEncontradoExcecao>(async () => await casoDeUso.Executar(requisicao, cancellationToken));
        Assert.NotNull(ex);
        var mensagensDeErro = ex.PegarMensagensDeErro();
        Assert.Single(mensagensDeErro);
        Assert.Contains(MensagensExcecao.USUARIO_NAO_ENCONTRADO, mensagensDeErro);
    }

    private static PegarUsuarioPorId CriarCasoDeUso(
        CancellationToken cancellationToken,
        GrowdevApi.Dominio.Entidades.Usuario usuarioLogado,
        GrowdevApi.Dominio.Entidades.Usuario? usuarioPegar = null)
    {
        var repository = new RepositorioUsuarioBuilder();

        if (usuarioPegar != null) repository.SetupPegarUsuarioPorIdReturnsUsuario(usuarioPegar, cancellationToken);

        return new PegarUsuarioPorId(
            repository.Build(),
            MapperBuilder.Build()
        );
    }
}
