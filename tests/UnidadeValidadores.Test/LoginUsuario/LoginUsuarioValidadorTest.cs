using GrowdevApi.Aplicacao.CasosDeUso.LoginUsuario;
using GrowdevApi.Excecao;
using TestUtil.Requisicoes;

namespace UnidadeValidadores.Test.LoginUsuario;

public class LoginUsuarioValidadorTest
{

    [Fact]
    public void Sucesso()
    {
        var validador = new FazerLoginUsuarioValidador();

        var requisicao = RequisicaoLoginUsuarioBuilder.Build();

        var resultado = validador.Validate(requisicao);

        Assert.True(resultado.IsValid);
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("        ")]
    public void Erro_Email_Vazio(string? email)
    {
        var validador = new FazerLoginUsuarioValidador();

        var requisicao = RequisicaoLoginUsuarioBuilder.Build();
        requisicao.Email = email;

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
        var validador = new FazerLoginUsuarioValidador();

        var requisicao = RequisicaoLoginUsuarioBuilder.Build();
        requisicao.Senha = senha;

        var resultado = validador.Validate(requisicao);

        Assert.False(resultado.IsValid);
        Assert.Single(resultado.Errors);
        Assert.Equal(MensagensExcecao.SENHA_VAZIA, resultado.Errors[0].ErrorMessage);
    }
}
