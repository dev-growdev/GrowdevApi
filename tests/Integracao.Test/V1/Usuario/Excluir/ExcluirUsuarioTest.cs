using GrowdevApi.Excecao;
using Integracao.Test.InfraestruturaEmMemoria;
using System.Net;
using TestUtil.HttpUtil;
using TestUtil.Requisicoes;
using TestUtil.Tokens;

namespace Integracao.Test.V1.Usuario.Excluir;

public class ExcluirUsuarioTest : GrowdevApiClassFixture
{
    private readonly string _baseUrl = "api/v1/Usuario";
    private readonly GrowdevApi.Dominio.Entidades.Usuario _usuarioAdministrador;

    public ExcluirUsuarioTest()
    {
        _usuarioAdministrador = (GrowdevApi.Dominio.Entidades.Usuario)_factory.EntidadesCriadas()["UsuarioAdministrador"];
    }

    [Fact]
    public async Task Sucesso()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var token = GeradorTokenUsuarioBuilder.Build().Gerar(_usuarioAdministrador.Id);

        var novoUsuario = await _cadastroHelper.CadastrarNovoUsuario(_usuarioAdministrador);

        var resposta = await _httpHelper.DoGet(url: $"{_baseUrl}/{novoUsuario.Id}", cancellationToken: cancellationToken, token: token);

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);

        resposta = await _httpHelper.DoDelete(url: $"{_baseUrl}/{novoUsuario.Id}", cancellationToken: cancellationToken, token: token);

        Assert.Equal(HttpStatusCode.NoContent, resposta.StatusCode);

        resposta = await _httpHelper.DoGet(url: $"{_baseUrl}/{novoUsuario.Id}", cancellationToken: cancellationToken, token: token);

        Assert.Equal(HttpStatusCode.NotFound, resposta.StatusCode);
    }

    [Fact]
    public async Task Erro_Auto_Exclusao()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var token = GeradorTokenUsuarioBuilder.Build().Gerar(_usuarioAdministrador.Id);

        var resposta = await _httpHelper.DoDelete(url: $"{_baseUrl}/{_usuarioAdministrador.Id}", cancellationToken: cancellationToken, token: token);

        Assert.Equal(HttpStatusCode.Forbidden, resposta.StatusCode);

        var erros = await HttpResponseUtil.PegarMensagensDeErro(resposta);

        var mensagemEsperada = MensagensExcecao.AUTO_EXCLUSAO;

        Assert.Single(erros);
        Assert.Equal(mensagemEsperada, erros.FirstOrDefault().GetString());
    }

    [Fact]
    public async Task Erro_Usuario_Nao_Encontrado()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var token = GeradorTokenUsuarioBuilder.Build().Gerar(_usuarioAdministrador.Id);

        var response = await _httpHelper.DoDelete(url: $"{_baseUrl}/{Guid.NewGuid()}", cancellationToken: cancellationToken, token: token);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var erros = await HttpResponseUtil.PegarMensagensDeErro(response);

        var mensagemEsperada = MensagensExcecao.USUARIO_NAO_ENCONTRADO;

        Assert.Single(erros);
        Assert.Equal(mensagemEsperada, erros.FirstOrDefault().GetString());
    }

    [Fact]
    public async Task Erro_Token_Invalido()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var response = await _httpHelper.DoDelete($"{_baseUrl}/{_usuarioAdministrador.Id}", cancellationToken, "tokenInvalid");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var erros = await HttpResponseUtil.PegarMensagensDeErro(response);

        var mensagemEsperada = MensagensExcecao.TOKEN_INVALIDO;

        Assert.Single(erros);
        Assert.Equal(mensagemEsperada, erros.FirstOrDefault().GetString());
    }

    [Theory]
    [InlineData("123")]
    [InlineData("asdfasdf")]
    [InlineData("b9fc55af-e38u-4852-b4f5-ad6b1277472d")]
    public async Task Erro_Id_Invalido(string id)
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var token = GeradorTokenUsuarioBuilder.Build().Gerar(_usuarioAdministrador.Id);

        var response = await _httpHelper.DoDelete(url: $"{_baseUrl}/{id}", cancellationToken: cancellationToken, token: token);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var erros = await HttpResponseUtil.PegarMensagensDeErro(response);

        var mensagemEsperada = MensagensExcecao.ID_INVALIDO;

        Assert.Single(erros);
        Assert.Equal(mensagemEsperada, erros.FirstOrDefault().GetString());
    }

    [Fact]
    public async Task Erro_Sem_Token()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var response = await _httpHelper.DoDelete($"{_baseUrl}/{_usuarioAdministrador.Id}", cancellationToken);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var erros = await HttpResponseUtil.PegarMensagensDeErro(response);

        var mensagemEsperada = MensagensExcecao.SEM_TOKEN;

        Assert.Single(erros);
        Assert.Equal(mensagemEsperada, erros.FirstOrDefault().GetString());
    }

    [Fact]
    public async Task Erro_Token_De_Usuario_Nao_Encontrado()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var token = GeradorTokenUsuarioBuilder.Build().Gerar(Guid.NewGuid());

        var response = await _httpHelper.DoDelete($"{_baseUrl}/{_usuarioAdministrador.Id}", cancellationToken, token);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var erros = await HttpResponseUtil.PegarMensagensDeErro(response);

        var mensagemEsperada = MensagensExcecao.TOKEN_INVALIDO;

        Assert.Single(erros);
        Assert.Equal(mensagemEsperada, erros.FirstOrDefault().GetString());
    }

    [Fact]
    public async Task Erro_Token_Expirado()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var token = _factory.Configuration!["DadosTeste:TokenDeUsuarioExpirado"];

        var requisicao = RequisicaoAlterarUsuarioBuilder.Build();

        var resposta = await _httpHelper.DoPut(_baseUrl, cancellationToken, requisicao, token!);

        Assert.Equal(HttpStatusCode.Unauthorized, resposta.StatusCode);

        var dadosDaResposta = await HttpResponseUtil.PegarDadosDaResposta(resposta);

        var erros = dadosDaResposta.RootElement.GetProperty("mensagensDeErro").EnumerateArray();

        var mensagemEsperada = MensagensExcecao.TOKEN_EXPIRADO;

        Assert.Single(erros);
        Assert.Equal(mensagemEsperada, erros.FirstOrDefault().GetString());
        Assert.True(dadosDaResposta.RootElement.GetProperty("tokenEstaExpirado").GetBoolean());
    }
}
