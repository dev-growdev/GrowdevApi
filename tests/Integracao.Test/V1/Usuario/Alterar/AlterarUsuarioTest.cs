using GrowdevApi.Dominio.Enums;
using GrowdevApi.Excecao;
using Integracao.Test.InfraestruturaEmMemoria;
using System.Net;
using TestUtil.HttpUtil;
using TestUtil.Requisicoes;
using TestUtil.Tokens;

namespace Integracao.Test.V1.Usuario.Alterar;

public class AlterarUsuarioTest : GrowdevApiClassFixture
{
    private readonly string _baseUrl = "api/v1/Usuario";
    private readonly GrowdevApi.Dominio.Entidades.Usuario _usuarioAdministrador;

    public AlterarUsuarioTest()
    {
        _usuarioAdministrador = (GrowdevApi.Dominio.Entidades.Usuario)_factory.EntidadesCriadas()["UsuarioAdministrador"];
    }

    [Fact]
    public async Task Sucesso()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var token = GeradorTokenUsuarioBuilder.Build().Gerar(_usuarioAdministrador.Id);

        var novoUsuario = await _cadastroHelper.CadastrarNovoUsuario(_usuarioAdministrador);

        var requisicao = RequisicaoAlterarUsuarioBuilder.Build(novoUsuario);
        requisicao.Nome += " da Silva";
        requisicao.Email = $"alterado{Guid.NewGuid()}@email.com";

        var resposta = await _httpHelper.DoPut(url: _baseUrl, cancellationToken: cancellationToken, requisicao: requisicao, token: token);

        Assert.Equal(HttpStatusCode.NoContent, resposta.StatusCode);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("asdfasdf")]
    [InlineData("b9fc55af-e38u-4852-b4f5-ad6b1277472d")]
    public async Task Erro_Id_Invalido(string id)
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var token = GeradorTokenUsuarioBuilder.Build().Gerar(_usuarioAdministrador.Id);

        var requisicao = RequisicaoAlterarUsuarioBuilder.Build();
        requisicao.Id = id;

        var resposta = await _httpHelper.DoPut(url: _baseUrl, cancellationToken: cancellationToken, requisicao: requisicao, token: token);

        Assert.Equal(HttpStatusCode.BadRequest, resposta.StatusCode);

        var erros = await HttpResponseUtil.PegarMensagensDeErro(resposta);

        var mensagemEsperada = MensagensExcecao.ID_INVALIDO;

        Assert.Single(erros);
        Assert.Equal(mensagemEsperada, erros.FirstOrDefault().GetString());
    }

    [Fact]
    public async Task Erro_Usuario_Nao_Encontrado()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var token = GeradorTokenUsuarioBuilder.Build().Gerar(_usuarioAdministrador.Id);

        var requisicao = RequisicaoAlterarUsuarioBuilder.Build();

        var resposta = await _httpHelper.DoPut(url: _baseUrl, cancellationToken: cancellationToken, requisicao: requisicao, token: token);

        Assert.Equal(HttpStatusCode.NotFound, resposta.StatusCode);

        var erros = await HttpResponseUtil.PegarMensagensDeErro(resposta);

        var mensagemEsperada = MensagensExcecao.USUARIO_NAO_ENCONTRADO;

        Assert.Single(erros);
        Assert.Equal(mensagemEsperada, erros.FirstOrDefault().GetString());
    }

    [Fact]
    public async Task Erro_Sem_Token()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var requisicao = RequisicaoAlterarUsuarioBuilder.Build();

        var resposta = await _httpHelper.DoPut(_baseUrl, cancellationToken, requisicao);

        Assert.Equal(HttpStatusCode.Unauthorized, resposta.StatusCode);

        var erros = await HttpResponseUtil.PegarMensagensDeErro(resposta);

        var mensagemEsperada = MensagensExcecao.SEM_TOKEN;

        Assert.Single(erros);
        Assert.Equal(mensagemEsperada, erros.FirstOrDefault().GetString());
    }

    [Fact]
    public async Task Erro_Token_Invalido()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var requisicao = RequisicaoAlterarUsuarioBuilder.Build();

        var resposta = await _httpHelper.DoPut(_baseUrl, cancellationToken, requisicao, "TokenInvalido");

        Assert.Equal(HttpStatusCode.Unauthorized, resposta.StatusCode);

        var erros = await HttpResponseUtil.PegarMensagensDeErro(resposta);

        var mensagemEsperada = MensagensExcecao.TOKEN_INVALIDO;

        Assert.Single(erros);
        Assert.Equal(mensagemEsperada, erros.FirstOrDefault().GetString());
    }

    [Fact]
    public async Task Erro_Token_De_Usuario_Nao_Encontrado()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var token = GeradorTokenUsuarioBuilder.Build().Gerar(Guid.NewGuid());

        var requisicao = RequisicaoAlterarUsuarioBuilder.Build();

        var resposta = await _httpHelper.DoPut(_baseUrl, cancellationToken, requisicao, token);

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

    [Fact]
    public async Task Erro_Email_Ja_Existe()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var token = GeradorTokenUsuarioBuilder.Build().Gerar(_usuarioAdministrador.Id);

        var novoUsuario = await _cadastroHelper.CadastrarNovoUsuario(_usuarioAdministrador);

        var requisicao = RequisicaoAlterarUsuarioBuilder.Build(novoUsuario);
        requisicao.Email = _usuarioAdministrador.Email;

        var resposta = await _httpHelper.DoPut(url: _baseUrl, cancellationToken: cancellationToken, requisicao: requisicao, token: token);

        Assert.Equal(HttpStatusCode.BadRequest, resposta.StatusCode);

        var erros = await HttpResponseUtil.PegarMensagensDeErro(resposta);

        var mensagemEsperada = MensagensExcecao.EMAIL_JA_EXISTE;

        Assert.Single(erros);
        Assert.Equal(mensagemEsperada, erros.FirstOrDefault().GetString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("        ")]
    public async Task Erro_Email_Vazio(string? email)
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var token = GeradorTokenUsuarioBuilder.Build().Gerar(_usuarioAdministrador.Id);

        var novoUsuario = await _cadastroHelper.CadastrarNovoUsuario(_usuarioAdministrador);

        var requisicao = RequisicaoAlterarUsuarioBuilder.Build(novoUsuario);
        requisicao.Email = "";

        var resposta = await _httpHelper.DoPut(_baseUrl, cancellationToken, requisicao, token);

        Assert.Equal(HttpStatusCode.BadRequest, resposta.StatusCode);

        var erros = await HttpResponseUtil.PegarMensagensDeErro(resposta);

        var mensagemEsperada = MensagensExcecao.EMAIL_VAZIO;

        Assert.Single(erros);
        Assert.Equal(mensagemEsperada, erros.FirstOrDefault().GetString());
    }

    [Theory]
    [InlineData("Suspenso")]
    [InlineData("asdfasdf")]
    [InlineData("Excluido")]
    public async Task Erro_Status_Invalido(string status)
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var token = GeradorTokenUsuarioBuilder.Build().Gerar(_usuarioAdministrador.Id);

        var requisicao = RequisicaoAlterarUsuarioBuilder.Build(_usuarioAdministrador);
        requisicao.Status = status;

        var resposta = await _httpHelper.DoPut(url: _baseUrl, cancellationToken: cancellationToken, requisicao: requisicao, token: token);

        Assert.Equal(HttpStatusCode.BadRequest, resposta.StatusCode);

        var erros = await HttpResponseUtil.PegarMensagensDeErro(resposta);

        var mensagemEsperada = MensagensExcecao.STATUS_INVALIDO.Replace("{ValoresPossiveis}", EnumUtil.PegarNomesEnumSeparadosPorVirgula<StatusUsuario>(["Indefinido"]));

        Assert.Single(erros);
        Assert.Equal(mensagemEsperada, erros.FirstOrDefault().GetString());
    }
}
