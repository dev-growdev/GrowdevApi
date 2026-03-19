using GrowdevApi.Excecao;
using Integracao.Test.InfraestruturaEmMemoria;
using System.Net;
using TestUtil.HttpUtil;
using TestUtil.Requisicoes;

namespace Integracao.Test.V1.LoginUsuario;

public class LoginUsuarioTest : GrowdevApiClassFixture
{
    private readonly string _baseUrl = "api/v1/login";
    private readonly GrowdevApi.Dominio.Entidades.Usuario _usuarioAdministrador;

    public LoginUsuarioTest()
    {
        _usuarioAdministrador = (GrowdevApi.Dominio.Entidades.Usuario)_factory.EntidadesCriadas()["UsuarioAdministrador"];
    }

    [Fact]
    public async Task Sucesso()
    {
        var cancellationToken = new CancellationTokenSource().Token;
        var requisicao = RequisicaoLoginUsuarioBuilder.Build(_usuarioAdministrador);
        var resposta = await _httpHelper.DoPost(_baseUrl, cancellationToken, requisicao);
        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        var dadosDaResposta = await HttpResponseUtil.PegarDadosDaResposta(resposta);

        var name = dadosDaResposta.RootElement.GetProperty("nome").GetString();
        Assert.False(string.IsNullOrWhiteSpace(name));

        var accessToken = dadosDaResposta.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString();
        Assert.False(string.IsNullOrWhiteSpace(accessToken));

        var refreshToken = dadosDaResposta.RootElement.GetProperty("tokens").GetProperty("refreshToken").GetString();
        Assert.False(string.IsNullOrWhiteSpace(refreshToken));
    }

    [Fact]
    public async Task Erro_Login_Invalido()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var requisicao = RequisicaoLoginUsuarioBuilder.Build();
        var resposta = await _httpHelper.DoPost(_baseUrl, cancellationToken, requisicao);

        Assert.Equal(HttpStatusCode.Unauthorized, resposta.StatusCode);

        var erros = await HttpResponseUtil.PegarMensagensDeErro(resposta);
        var mensagemEsperada = MensagensExcecao.EMAIL_OU_SENHA_INVALIDOS;

        var errosList = erros.ToList();
        Assert.Single(errosList);
        Assert.Equal(mensagemEsperada, errosList.FirstOrDefault().GetString());
    }

}
