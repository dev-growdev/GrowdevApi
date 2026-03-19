using GrowdevApi.Aplicacao.CasosDeUso.Usuario.Excluir;
using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Excecao;
using TestUtil.Entidades;
using TestUtil.Repositorios;
using TestUtil.Servicos;

namespace UnidadeCasosDeUso.Test.Usuario.Excluir;

public class ExcluirUsuarioTest
{
    [Fact]
    public async Task Sucesso()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var usuarioLogado = UsuarioBuilder.Build();
        var usuarioExclusao = UsuarioBuilder.Build();

        var requisicao = new RequisicaoExcluirUsuario()
        {
            Id = usuarioExclusao.Id.ToString(),
        };

        var casoDeUso = CriarCasoDeUso(cancellationToken: cancellationToken, usuarioLogado: usuarioLogado, usuarioExclusao: usuarioExclusao);

        async Task func() => await casoDeUso.Executar(requisicao, cancellationToken);

        var exception = await Record.ExceptionAsync(func);
        Assert.Null(exception);
    }

    [Fact]
    public async Task Erro_Auto_Exclusao()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var usuarioLogado = UsuarioBuilder.Build();

        var requisicao = new RequisicaoExcluirUsuario()
        {
            Id = usuarioLogado.Id.ToString()
        };

        var casoDeUso = CriarCasoDeUso(cancellationToken: cancellationToken, usuarioLogado: usuarioLogado, usuarioExclusao: usuarioLogado);

        async Task func() => await casoDeUso.Executar(requisicao, cancellationToken);

        var ex = await Assert.ThrowsAsync<ProibidoExcecao>(async () => await func());
        Assert.NotNull(ex);
        var mensagensDeErro = ex!.PegarMensagensDeErro();
        Assert.Single(mensagensDeErro);
        Assert.Contains(MensagensExcecao.AUTO_EXCLUSAO, mensagensDeErro);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("asdfasdf")]
    [InlineData("b9fc55af-e38u-4852-b4f5-ad6b1277472d")]
    public async Task Erro_Id_Invalido(string id)
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var usuarioLogado = UsuarioBuilder.Build();

        var requisicao = new RequisicaoExcluirUsuario()
        {
            Id = id,
        };

        var casoDeUso = CriarCasoDeUso(cancellationToken: cancellationToken, usuarioLogado: usuarioLogado, usuarioExclusao: usuarioLogado);

        async Task func() => await casoDeUso.Executar(requisicao, cancellationToken);

        var ex = await Assert.ThrowsAsync<ErroValidacaoExcecao>(async () => await func());
        Assert.NotNull(ex);
        var mensagensDeErro = ex!.PegarMensagensDeErro();
        Assert.Single(mensagensDeErro);
        Assert.Contains(MensagensExcecao.ID_INVALIDO, mensagensDeErro);
    }

    [Fact]
    public async Task Erro_Usuario_Nao_Encontrado()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var usuarioLogado = UsuarioBuilder.Build();

        var requisicao = new RequisicaoExcluirUsuario()
        {
            Id = Guid.NewGuid().ToString(),
        };

        var casoDeUso = CriarCasoDeUso(cancellationToken: cancellationToken, usuarioLogado: usuarioLogado);

        async Task func() => await casoDeUso.Executar(requisicao, cancellationToken);

        var ex = await Assert.ThrowsAsync<NaoEncontradoExcecao>(async () => await func());
        Assert.NotNull(ex);
        var mensagensDeErro = ex!.PegarMensagensDeErro();
        Assert.Single(mensagensDeErro);
        Assert.Contains(MensagensExcecao.USUARIO_NAO_ENCONTRADO, mensagensDeErro);
    }

    private static ExcluirUsuario CriarCasoDeUso(
        CancellationToken cancellationToken,
        GrowdevApi.Dominio.Entidades.Usuario usuarioLogado,
        GrowdevApi.Dominio.Entidades.Usuario? usuarioExclusao = null)
    {
        var repository = new RepositorioUsuarioBuilder();

        if (usuarioExclusao != null) repository.SetupPegarUsuarioPorIdReturnsUsuario(usuarioExclusao, cancellationToken);

        return new ExcluirUsuario(
            UsuarioLogadoServicoBuilder.Build(usuarioLogado, cancellationToken),
            repository.Build(),
            UnitOfWorkBuilder.Build());
    }
}
