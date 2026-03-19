using GrowdevApi.Dominio.Enums;
using System.Net;
using TestUtil.HttpUtil;
using TestUtil.Requisicoes;
using TestUtil.Tokens;

namespace Integracao.Test.Helpers;

public class CadastroHelper
{
    private readonly HttpHelper _httpHelper;

    public CadastroHelper(
        HttpHelper httpHelper)
    {
        _httpHelper = httpHelper;
    }

    public async Task<GrowdevApi.Dominio.Entidades.Usuario> CadastrarNovoUsuario(GrowdevApi.Dominio.Entidades.Usuario usuarioLogado, string versaoApi = "v1")
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var token = GeradorTokenUsuarioBuilder.Build().Gerar(usuarioLogado.Id);

        var requisicao = RequisicaoCadastrarUsuarioBuilder.Build();

        var resposta = await _httpHelper.DoPost($"api/{versaoApi}/usuario", cancellationToken, requisicao, token);

        Assert.Equal(HttpStatusCode.Created, resposta.StatusCode);

        var dadosDaResposta = await HttpResponseUtil.PegarDadosDaResposta(resposta);

        var id = dadosDaResposta.RootElement.GetProperty("id").GetString();
        Assert.False(string.IsNullOrWhiteSpace(id));

        var nome = dadosDaResposta.RootElement.GetProperty("nome").GetString();
        Assert.False(string.IsNullOrWhiteSpace(nome));
        Assert.Equal(requisicao.Nome, nome);

        var email = dadosDaResposta.RootElement.GetProperty("email").GetString();
        Assert.False(string.IsNullOrWhiteSpace(email));
        Assert.Equal(requisicao.Email, email);

        var status = dadosDaResposta.RootElement.GetProperty("status").GetString();
        Assert.False(string.IsNullOrWhiteSpace(status));
        Assert.Equal("Ativo", status);

        return new GrowdevApi.Dominio.Entidades.Usuario
        {
            Id = Guid.Parse(id),
            Nome = nome,
            Email = email,
            Senha = requisicao.Senha,
            Status = EnumUtil.ConverterTextoParaEnum<StatusUsuario>(status)
        };
    }
}
