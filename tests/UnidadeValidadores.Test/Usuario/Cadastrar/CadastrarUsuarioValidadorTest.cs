using GrowdevApi.Aplicacao.CasosDeUso.Usuario.Cadastrar;
using GrowdevApi.Excecao;
using TestUtil.Requisicoes;

namespace UnidadeValidadores.Test.Usuario.Cadastrar;

public class CadastrarUsuarioValidadorTest
{
    [Fact]
    public void Sucesso()
    {
        var validador = new CadastrarUsuarioValidador();
        var requisicao = RequisicaoCadastrarUsuarioBuilder.Build();
        var resultado = validador.Validate(requisicao);
        Assert.True(resultado.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("        ")]
    public void Erro_Nome_Vazio(string? nome)
    {
        var validador = new CadastrarUsuarioValidador();
        var requisicao = RequisicaoCadastrarUsuarioBuilder.Build();
        requisicao.Nome = nome;
        var resultado = validador.Validate(requisicao);
        Assert.False(resultado.IsValid);
        Assert.Single(resultado.Errors);
        Assert.Equal(MensagensExcecao.NOME_VAZIO, resultado.Errors[0].ErrorMessage);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("        ")]
    public void Erro_Email_Vazio(string? email)
    {
        var validador = new CadastrarUsuarioValidador();
        var requisicao = RequisicaoCadastrarUsuarioBuilder.Build();
        requisicao.Email = email!;
        var resultado = validador.Validate(requisicao);
        Assert.False(resultado.IsValid);
        Assert.Single(resultado.Errors);
        Assert.Equal(MensagensExcecao.EMAIL_VAZIO, resultado.Errors[0].ErrorMessage);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("        ")]
    public void Erro_Senha_Vazia(string? senha)
    {
        var validador = new CadastrarUsuarioValidador();
        var requisicao = RequisicaoCadastrarUsuarioBuilder.Build();
        requisicao.Senha = senha;
        var resultado = validador.Validate(requisicao);
        Assert.False(resultado.IsValid);
        Assert.Single(resultado.Errors);
        Assert.Equal(MensagensExcecao.SENHA_VAZIA, resultado.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Erro_Email_Invalido()
    {
        var validador = new CadastrarUsuarioValidador();
        var requisicao = RequisicaoCadastrarUsuarioBuilder.Build();
        requisicao.Email = "AlgumLixoAqui";
        var resultado = validador.Validate(requisicao);
        Assert.False(resultado.IsValid);
        Assert.Single(resultado.Errors);
        Assert.Equal(MensagensExcecao.EMAIL_INVALIDO, resultado.Errors[0].ErrorMessage);
    }

    [Theory]
    [InlineData("1234")]
    [InlineData("12345678")]
    [InlineData("soletraminuscula")]
    [InlineData("SOLETRAMAIUSCULA")]
    public void Erro_Senha_Invalida(string? senha)
    {
        var validador = new CadastrarUsuarioValidador();
        var requisicao = RequisicaoCadastrarUsuarioBuilder.Build();
        requisicao.Senha = senha!;
        var resultado = validador.Validate(requisicao);
        Assert.False(resultado.IsValid);
        Assert.Single(resultado.Errors);
        Assert.Equal(MensagensExcecao.SENHA_INVALIDA, resultado.Errors[0].ErrorMessage);
    }
}
