using GrowdevApi.Excecao;
using Integracao.Test.InfraestruturaEmMemoria;
using System.Net;
using TestUtil.HttpUtil;
using TestUtil.Tokens;

namespace Integracao.Test.V1.Usuario.Pegar;

public class PegarUsuarioLogadoTest : GrowdevApiClassFixture
{
    private readonly string _baseUrl = "api/v1/Usuario";
    private readonly GrowdevApi.Dominio.Entidades.Usuario _usuarioAdministrador;

    public PegarUsuarioLogadoTest()
    {
        _usuarioAdministrador = (GrowdevApi.Dominio.Entidades.Usuario)_factory.EntidadesCriadas()["UsuarioAdministrador"];
    }

    [Fact]
    public async Task Sucesso()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var token = GeradorTokenUsuarioBuilder.Build().Gerar(_usuarioAdministrador.Id);

        var resposta = await _httpHelper.DoGet(_baseUrl, cancellationToken, token);

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        var dadosDaResposta = await HttpResponseUtil.PegarDadosDaResposta(resposta);

        var id = dadosDaResposta.RootElement.GetProperty("id").GetString();
        Assert.False(string.IsNullOrWhiteSpace(id));
        Assert.Equal(_usuarioAdministrador.Id.ToString(), id);

        var nome = dadosDaResposta.RootElement.GetProperty("nome").GetString();
        Assert.False(string.IsNullOrWhiteSpace(nome));
        Assert.Equal(_usuarioAdministrador.Nome, nome);

        var email = dadosDaResposta.RootElement.GetProperty("email").GetString();
        Assert.False(string.IsNullOrWhiteSpace(email));
        Assert.Equal(_usuarioAdministrador.Email, email);

        var status = dadosDaResposta.RootElement.GetProperty("status").GetString();
        Assert.False(string.IsNullOrWhiteSpace(status));
        Assert.Equal(_usuarioAdministrador.Status.ToString(), status);
    }

    [Fact]
    public async Task Erro_Token_Invalido()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var resposta = await _httpHelper.DoGet(_baseUrl, cancellationToken, "tokenInvalid");

        Assert.Equal(HttpStatusCode.Unauthorized, resposta.StatusCode);

        var erros = await HttpResponseUtil.PegarMensagensDeErro(resposta);

        var mensagemEsperada = MensagensExcecao.TOKEN_INVALIDO;

        Assert.Single(erros);
        Assert.Equal(mensagemEsperada, erros.FirstOrDefault().GetString());
    }

    [Fact]
    public async Task Erro_Sem_Token()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var resposta = await _httpHelper.DoGet(_baseUrl, cancellationToken);

        Assert.Equal(HttpStatusCode.Unauthorized, resposta.StatusCode);

        var erros = await HttpResponseUtil.PegarMensagensDeErro(resposta);

        var mensagemEsperada = MensagensExcecao.SEM_TOKEN;

        Assert.Single(erros);
        Assert.Equal(mensagemEsperada, erros.FirstOrDefault().GetString());
    }

    [Fact]
    public async Task Erro_Token_De_Usuario_Nao_Encontrado()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var token = GeradorTokenUsuarioBuilder.Build().Gerar(Guid.NewGuid());

        var resposta = await _httpHelper.DoGet(_baseUrl, cancellationToken, token);

        Assert.Equal(HttpStatusCode.Unauthorized, resposta.StatusCode);

        var erros = await HttpResponseUtil.PegarMensagensDeErro(resposta);

        var mensagemEsperada = MensagensExcecao.TOKEN_INVALIDO;

        Assert.Single(erros);
        Assert.Equal(mensagemEsperada, erros.FirstOrDefault().GetString());
    }

    [Fact]
    public async Task Erro_Token_Expirado()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var token = _factory.Configuration!["DadosTeste:TokenDeUsuarioExpirado"];

        var resposta = await _httpHelper.DoGet(_baseUrl, cancellationToken, token!);

        Assert.Equal(HttpStatusCode.Unauthorized, resposta.StatusCode);

        var dadosDaResposta = await HttpResponseUtil.PegarDadosDaResposta(resposta);

        var erros = dadosDaResposta.RootElement.GetProperty("mensagensDeErro").EnumerateArray();

        var mensagemEsperada = MensagensExcecao.TOKEN_EXPIRADO;

        Assert.Single(erros);
        Assert.Equal(mensagemEsperada, erros.FirstOrDefault().GetString());
        Assert.True(dadosDaResposta.RootElement.GetProperty("tokenEstaExpirado").GetBoolean());
    }
}
