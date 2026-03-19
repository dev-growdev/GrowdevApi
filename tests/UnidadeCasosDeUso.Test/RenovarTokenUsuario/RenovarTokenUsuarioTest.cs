using GrowdevApi.Dominio.Entidades;
using GrowdevApi.Excecao;
using TestUtil.Entidades;
using TestUtil.Repositorios;
using TestUtil.Requisicoes;
using TestUtil.Tokens;

namespace UnidadeCasosDeUso.Test.RenovarTokenUsuario;

public class RenovarTokenUsuarioTest
{
    [Fact]
    public async Task Sucesso()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var usuario = UsuarioBuilder.Build();

        var refreshToken = RefreshTokenBuilder.Build(usuario);

        var requisicao = RequisicaoNovoTokenUsuarioBuilder.Build(refreshToken);

        var casoDeUso = CriarCasoDeUso(cancellationToken: cancellationToken, refreshToken: refreshToken, true);

        var resposta = await casoDeUso.Executar(requisicao, cancellationToken);

        Assert.NotNull(resposta);
        Assert.False(string.IsNullOrWhiteSpace(resposta.AccessToken));
        Assert.False(string.IsNullOrWhiteSpace(resposta.RefreshToken));
    }

    [Fact]
    public async Task Erro_Token_Nao_Encontrado()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var usuario = UsuarioBuilder.Build();

        var refreshToken = RefreshTokenBuilder.Build(usuario);

        var requisicao = RequisicaoNovoTokenUsuarioBuilder.Build(refreshToken);

        var casoDeUso = CriarCasoDeUso(cancellationToken: cancellationToken);

        async Task func() => await casoDeUso.Executar(requisicao, cancellationToken);

        var ex = await Assert.ThrowsAsync<SessaoExpiradaExcecao>(func);
        Assert.NotNull(ex);
        var mensagensDeErro = ex!.PegarMensagensDeErro();
        Assert.Single(mensagensDeErro);
        Assert.Contains(MensagensExcecao.SESSAO_EXPIRADA, mensagensDeErro);
    }

    [Fact]
    public async Task Erro_Token_Expirado()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var usuario = UsuarioBuilder.Build();

        var refreshToken = RefreshTokenBuilder.Build(usuario);

        var requisicao = RequisicaoNovoTokenUsuarioBuilder.Build(refreshToken);

        var casoDeUso = CriarCasoDeUso(cancellationToken: cancellationToken, refreshToken: refreshToken, false);

        async Task func() => await casoDeUso.Executar(requisicao, cancellationToken);

        var ex = await Assert.ThrowsAsync<SessaoExpiradaExcecao>(func);
        Assert.NotNull(ex);
        var mensagensDeErro = ex!.PegarMensagensDeErro();
        Assert.Single(mensagensDeErro);
        Assert.Contains(MensagensExcecao.SESSAO_EXPIRADA, mensagensDeErro);
    }

    private static GrowdevApi.Aplicacao.CasosDeUso.RenovarTokenUsuario.RenovarTokenUsuario CriarCasoDeUso(
        CancellationToken cancellationToken,
        RefreshToken? refreshToken = null,
        bool tokenValido = false)
    {
        var refreshTokenRepository = new RepositorioRefreshTokenBuilder();
        var geradorRefreshToken = new GeradorRefreshTokenBuilder();

        if (refreshToken != null)
        {
            refreshTokenRepository.SetupPegarRefreshTokenReturnsRefreshToken(refreshToken.Valor, refreshToken, cancellationToken);
            if (tokenValido)
                geradorRefreshToken.SetupTokenValidoReturnsTrue(refreshToken);
        }

        return new GrowdevApi.Aplicacao.CasosDeUso.RenovarTokenUsuario.RenovarTokenUsuario(
            refreshTokenRepository.Build(),
            GeradorTokenUsuarioBuilder.Build(),
            geradorRefreshToken.Build(),
            UnitOfWorkBuilder.Build());
    }
}
