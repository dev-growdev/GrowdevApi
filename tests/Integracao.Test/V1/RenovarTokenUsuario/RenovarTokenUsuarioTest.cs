using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Excecao;
using Integracao.Test.InfraestruturaEmMemoria;
using System.Net;
using TestUtil.HttpUtil;
using TestUtil.Requisicoes;

namespace Integracao.Test.V1.RenovarTokenUsuario;

public class RenovarTokenUsuarioTest : GrowdevApiClassFixture
{
    private readonly string _baseUrlLogin = "api/v1/login";
    private readonly string _baseUrlRefreshToken = "api/v1/token/refresh-token";
    private readonly GrowdevApi.Dominio.Entidades.Usuario _usuarioAdministrador;

    public RenovarTokenUsuarioTest()
    {
        _usuarioAdministrador = (GrowdevApi.Dominio.Entidades.Usuario)_factory.EntidadesCriadas()["UsuarioAdministrador"];
    }

    private async Task<string> PegarRefreshTokenDoLogin()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var requisicaoLogin = RequisicaoLoginUsuarioBuilder.Build(_usuarioAdministrador);

        var respostaLogin = await _httpHelper.DoPost(_baseUrlLogin, cancellationToken, requisicaoLogin);

        Assert.Equal(HttpStatusCode.OK, respostaLogin.StatusCode);

        var respostaLoginData = await HttpResponseUtil.PegarDadosDaResposta(respostaLogin);

        return respostaLoginData.RootElement
            .GetProperty("tokens").GetProperty("refreshToken").GetString()!;
    }

    [Xunit.Fact]
    public async Task Sucesso()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var refreshTokenLogin = await PegarRefreshTokenDoLogin();

        var requisicao = new RequisicaoNovoTokenUsuario
        {
            RefreshToken = refreshTokenLogin!
        };

        var resposta = await _httpHelper.DoPost(_baseUrlRefreshToken, cancellationToken, requisicao);

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        var dadosDaResposta = await HttpResponseUtil.PegarDadosDaResposta(resposta);

        Assert.False(string.IsNullOrWhiteSpace(dadosDaResposta.RootElement.GetProperty("accessToken").GetString()));
        Assert.False(string.IsNullOrWhiteSpace(dadosDaResposta.RootElement.GetProperty("refreshToken").GetString()));
    }

    [Xunit.Fact]
    public async Task Erro_Refresh_Token_Nao_Encontrado()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var requisicaoNewToken = new RequisicaoNovoTokenUsuario
        {
            RefreshToken = "TOKEN_INVALIDO"
        };

        var respostaRefreshToken = await _httpHelper.DoPost(_baseUrlRefreshToken, cancellationToken, requisicaoNewToken);

        Assert.Equal(HttpStatusCode.Unauthorized, respostaRefreshToken.StatusCode);

        var errors = await HttpResponseUtil.PegarMensagensDeErro(respostaRefreshToken);

        var mensagemEsperada = MensagensExcecao.SESSAO_EXPIRADA;

        Assert.Single(errors);
        Assert.Equal(mensagemEsperada, errors.FirstOrDefault().GetString());
    }

}
