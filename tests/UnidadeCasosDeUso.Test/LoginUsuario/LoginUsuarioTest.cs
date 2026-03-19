namespace UnidadeCasosDeUso.Test.LoginUsuario;

using GrowdevApi.Aplicacao.CasosDeUso.LoginUsuario;
using GrowdevApi.Dominio.Entidades;
using GrowdevApi.Excecao;
using TestUtil.Criptografia;
using TestUtil.Entidades;
using TestUtil.Repositorios;
using TestUtil.Requisicoes;
using TestUtil.Tokens;
using Xunit;

public class LoginUsuarioTest
{
    [Fact]
    public async Task Sucesso()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var usuario = UsuarioBuilder.Build();

        var requisicao = RequisicaoLoginUsuarioBuilder.Build(usuario);

        usuario.Senha = EncriptadorSenhaBuilder.Build().Encriptar(usuario.Senha!);

        var casoDeUso = CriarCasoDeUso(usuario, cancellationToken);

        var resposta = await casoDeUso.Executar(requisicao, cancellationToken);

        Assert.NotNull(resposta);
        Assert.False(string.IsNullOrWhiteSpace(resposta.Nome));
        Assert.Equal(usuario.Nome, resposta.Nome);
        Assert.False(string.IsNullOrWhiteSpace(resposta.Tokens.AccessToken));
        Assert.False(string.IsNullOrWhiteSpace(resposta.Tokens.RefreshToken));
    }

    [Fact]
    public async Task Erro_Usuario_Nao_Existe()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var usuario = UsuarioBuilder.Build();

        var requisicao = RequisicaoLoginUsuarioBuilder.Build(usuario);

        usuario.Senha = EncriptadorSenhaBuilder.Build().Encriptar(usuario.Senha!);

        var casoDeUso = CriarCasoDeUso();

        async Task func() => await casoDeUso.Executar(requisicao, cancellationToken);

        var ex = await Assert.ThrowsAsync<LoginUsuarioInvalidoExcecao>(func);
        Assert.NotNull(ex);
        var mensagensDeErro = ex!.PegarMensagensDeErro();
        Assert.Single(mensagensDeErro);
        Assert.Contains(MensagensExcecao.EMAIL_OU_SENHA_INVALIDOS, mensagensDeErro);
    }

    [Fact]
    public async Task Erro_Senha_Invalida()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var usuario = UsuarioBuilder.Build();

        var requisicao = RequisicaoLoginUsuarioBuilder.Build(usuario);

        usuario.Senha = EncriptadorSenhaBuilder.Build().Encriptar(usuario.Senha!);

        requisicao.Senha = "SenhaInvalida";

        var casoDeUso = CriarCasoDeUso(usuario, cancellationToken);

        async Task func() => await casoDeUso.Executar(requisicao, cancellationToken);

        var ex = await Assert.ThrowsAsync<LoginUsuarioInvalidoExcecao>(func);
        Assert.NotNull(ex);
        var mensagensDeErro = ex!.PegarMensagensDeErro();
        Assert.Single(mensagensDeErro);
        Assert.Contains(MensagensExcecao.EMAIL_OU_SENHA_INVALIDOS, mensagensDeErro);
    }

    private static FazerLoginUsuario CriarCasoDeUso(
        Usuario? usuario = null,
        CancellationToken cancellationToken = default)
    {
        var usuarioRepository = new RepositorioUsuarioBuilder();

        if (usuario != null)
            usuarioRepository.SetupPegarUsuarioPorEmailReturnsUsuario(usuario, cancellationToken);

        return new FazerLoginUsuario(
            usuarioRepository.Build(),
            EncriptadorSenhaBuilder.Build(),
            GeradorTokenUsuarioBuilder.Build(),
            new GeradorRefreshTokenBuilder().Build(),
            new RepositorioRefreshTokenBuilder().Build(),
            UnitOfWorkBuilder.Build());
    }
}
