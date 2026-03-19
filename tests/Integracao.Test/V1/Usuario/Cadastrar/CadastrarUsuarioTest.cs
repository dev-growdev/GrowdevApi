using GrowdevApi.Excecao;
using Integracao.Test.InfraestruturaEmMemoria;
using System.Net;
using TestUtil.HttpUtil;
using TestUtil.Requisicoes;
using TestUtil.Tokens;

namespace Integracao.Test.V1.Usuario.Cadastrar;

public class CadastrarUsuarioTest : GrowdevApiClassFixture
{
    private readonly string _baseUrl = "api/v1/Usuario";
    private readonly GrowdevApi.Dominio.Entidades.Usuario _usuarioAdministrador;

    public CadastrarUsuarioTest()
    {
        _usuarioAdministrador = (GrowdevApi.Dominio.Entidades.Usuario)_factory.EntidadesCriadas()["UsuarioAdministrador"];
    }

    [Fact]
    public async Task Sucesso()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var token = GeradorTokenUsuarioBuilder.Build().Gerar(_usuarioAdministrador.Id);

        var requisicao = RequisicaoCadastrarUsuarioBuilder.Build();

        var resposta = await _httpHelper.DoPost(_baseUrl, cancellationToken, requisicao, token);

        Assert.Equal(HttpStatusCode.Created, resposta.StatusCode);

        var dadosDaResposta = await HttpResponseUtil.PegarDadosDaResposta(resposta);

        Assert.False(string.IsNullOrWhiteSpace(dadosDaResposta.RootElement.GetProperty("id").GetString()));

        var nome = dadosDaResposta.RootElement.GetProperty("nome").GetString();
        Assert.False(string.IsNullOrWhiteSpace(nome));
        Assert.Equal(requisicao.Nome, nome);

        var email = dadosDaResposta.RootElement.GetProperty("email").GetString();
        Assert.False(string.IsNullOrWhiteSpace(email));
        Assert.Equal(requisicao.Email, email);

        var status = dadosDaResposta.RootElement.GetProperty("status").GetString();
        Assert.False(string.IsNullOrWhiteSpace(status));
        Assert.Equal("Ativo", status);
    }

    [Fact]
    public async Task Erro_Sem_Token()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var requisicao = RequisicaoCadastrarUsuarioBuilder.Build();

        var resposta = await _httpHelper.DoPost(_baseUrl, cancellationToken, requisicao);

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

        var requisicao = RequisicaoCadastrarUsuarioBuilder.Build();

        var resposta = await _httpHelper.DoPost(_baseUrl, cancellationToken, requisicao, "TokenInvalido");

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

        var requisicao = RequisicaoCadastrarUsuarioBuilder.Build();

        var resposta = await _httpHelper.DoPost(_baseUrl, cancellationToken, requisicao, token);

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

        var requisicao = RequisicaoCadastrarUsuarioBuilder.Build();

        var resposta = await _httpHelper.DoPost(_baseUrl, cancellationToken, requisicao, token!);

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

        var requisicao = RequisicaoCadastrarUsuarioBuilder.Build();
        requisicao.Email = _usuarioAdministrador.Email;

        var resposta = await _httpHelper.DoPost(_baseUrl, cancellationToken, requisicao, token);

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
    public async Task Erro_Nome_Vazio(string? nome)
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var token = GeradorTokenUsuarioBuilder.Build().Gerar(_usuarioAdministrador.Id);

        var requisicao = RequisicaoCadastrarUsuarioBuilder.Build();
        requisicao.Nome = nome;

        var resposta = await _httpHelper.DoPost(_baseUrl, cancellationToken, requisicao, token);

        Assert.Equal(HttpStatusCode.BadRequest, resposta.StatusCode);

        var erros = await HttpResponseUtil.PegarMensagensDeErro(resposta);

        var mensagemEsperada = MensagensExcecao.NOME_VAZIO;

        Assert.Single(erros);
        Assert.Equal(mensagemEsperada, erros.FirstOrDefault().GetString());
    }
}
