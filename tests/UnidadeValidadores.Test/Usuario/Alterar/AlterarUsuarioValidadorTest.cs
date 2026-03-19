using GrowdevApi.Aplicacao.CasosDeUso.Usuario.Alterar;
using GrowdevApi.Dominio.Enums;
using GrowdevApi.Excecao;
using TestUtil.Requisicoes;

namespace UnidadeValidadores.Test.Usuario.Alterar;

public class AlterarUsuarioValidadorTest
{
    [Fact]
    public void Sucesso()
    {
        var validador = new AlterarUsuarioValidador();
        var requisicao = RequisicaoAlterarUsuarioBuilder.Build();
        var resultado = validador.Validate(requisicao);
        Assert.True(resultado.IsValid);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("        ")]
    public void Erro_Id_Vazio(string? id)
    {
        var validador = new AlterarUsuarioValidador();
        var requisicao = RequisicaoAlterarUsuarioBuilder.Build();
        requisicao.Id = id!;
        var resultado = validador.Validate(requisicao);
        Assert.False(resultado.IsValid);
        Assert.Single(resultado.Errors);
        Assert.Equal(MensagensExcecao.ID_VAZIO, resultado.Errors[0].ErrorMessage);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("asdfasdf")]
    [InlineData("b9fc55af-e38u-4852-b4f5-ad6b1277472d")]
    public void Erro_Id_Invalido(string id)
    {
        var validador = new AlterarUsuarioValidador();
        var requisicao = RequisicaoAlterarUsuarioBuilder.Build();
        requisicao.Id = id!;
        var resultado = validador.Validate(requisicao);
        Assert.False(resultado.IsValid);
        Assert.Single(resultado.Errors);
        Assert.Equal(MensagensExcecao.ID_INVALIDO, resultado.Errors[0].ErrorMessage);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("        ")]
    public void Erro_Nome_Vazio(string? nome)
    {
        var validador = new AlterarUsuarioValidador();
        var requisicao = RequisicaoAlterarUsuarioBuilder.Build();
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
        var validador = new AlterarUsuarioValidador();
        var requisicao = RequisicaoAlterarUsuarioBuilder.Build();
        requisicao.Email = email;
        var resultado = validador.Validate(requisicao);
        Assert.False(resultado.IsValid);
        Assert.Single(resultado.Errors);
        Assert.Equal(MensagensExcecao.EMAIL_VAZIO, resultado.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Erro_Email_Invalido()
    {
        var validador = new AlterarUsuarioValidador();
        var requisicao = RequisicaoAlterarUsuarioBuilder.Build();
        requisicao.Email = "AlgumLixoAqui";
        var resultado = validador.Validate(requisicao);
        Assert.False(resultado.IsValid);
        Assert.Single(resultado.Errors);
        Assert.Equal(MensagensExcecao.EMAIL_INVALIDO, resultado.Errors[0].ErrorMessage);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("        ")]
    public void Erro_Status_Vazio(string? status)
    {
        var validador = new AlterarUsuarioValidador();
        var requisicao = RequisicaoAlterarUsuarioBuilder.Build();
        requisicao.Status = status;
        var resultado = validador.Validate(requisicao);
        Assert.False(resultado.IsValid);
        Assert.Single(resultado.Errors);
        Assert.Equal(MensagensExcecao.STATUS_VAZIO, resultado.Errors[0].ErrorMessage);
    }

    [Theory]
    [InlineData("Removido")]
    [InlineData("asdgfasdf")]
    [InlineData("Excluido")]
    public void Erro_Status_Invalido(string? status)
    {
        var validador = new AlterarUsuarioValidador();
        var requisicao = RequisicaoAlterarUsuarioBuilder.Build();
        requisicao.Status = status;
        var resultado = validador.Validate(requisicao);
        Assert.False(resultado.IsValid);
        Assert.Single(resultado.Errors);
        Assert.Equal(MensagensExcecao.STATUS_INVALIDO.Replace("{ValoresPossiveis}", EnumUtil.PegarNomesEnumSeparadosPorVirgula<StatusUsuario>(["Indefinido"])), resultado.Errors[0].ErrorMessage);
    }
}
