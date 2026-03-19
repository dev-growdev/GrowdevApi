using GrowdevApi.Aplicacao.CasosDeUso.Usuario.Cadastrar;
using GrowdevApi.Excecao;
using TestUtil.AutoMapper;
using TestUtil.Criptografia;
using TestUtil.Entidades;
using TestUtil.Repositorios;
using TestUtil.Requisicoes;
using TestUtil.Servicos;

namespace UnidadeCasosDeUso.Test.Usuario.Cadastrar;

public class CadastrarUsuarioTest
{
    [Fact]
    public async Task Sucesso()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var requisicao = RequisicaoCadastrarUsuarioBuilder.Build();

        var casoDeUso = CriarCasoDeUso(cancellationToken);

        var resposta = await casoDeUso.Executar(requisicao, cancellationToken);

        Assert.NotNull(resposta);
        Assert.False(string.IsNullOrWhiteSpace(resposta.Nome));
        Assert.Equal(requisicao.Nome, resposta.Nome);
        Assert.False(string.IsNullOrWhiteSpace(resposta.Email));
        Assert.Equal(requisicao.Email, resposta.Email);
        Assert.False(string.IsNullOrWhiteSpace(resposta.Status));
        Assert.Equal("Ativo", resposta.Status);
    }

    [Fact]
    public async Task Erro_Email_Ja_Existe()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var requisicao = RequisicaoCadastrarUsuarioBuilder.Build();

        var casoDeUso = CriarCasoDeUso(cancellationToken, requisicao.Email);

        async Task func() => await casoDeUso.Executar(requisicao, cancellationToken);

        var ex = await Assert.ThrowsAsync<ErroValidacaoExcecao>(func);
        Assert.NotNull(ex);
        var mensagensDeErro = ex!.PegarMensagensDeErro();
        Assert.Single(mensagensDeErro);
        Assert.Contains(MensagensExcecao.EMAIL_JA_EXISTE, mensagensDeErro);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("        ")]
    public async Task Erro_Email_Vazio(string? email)
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var requisicao = RequisicaoCadastrarUsuarioBuilder.Build();
        requisicao.Email = email!;

        var casoDeUso = CriarCasoDeUso(cancellationToken);

        async Task func() => await casoDeUso.Executar(requisicao, cancellationToken);

        var ex = await Assert.ThrowsAsync<ErroValidacaoExcecao>(func);
        Assert.NotNull(ex);
        var mensagensDeErro = ex!.PegarMensagensDeErro();
        Assert.Single(mensagensDeErro);
        Assert.Contains(MensagensExcecao.EMAIL_VAZIO, mensagensDeErro);
    }

    private static CadastrarUsuario CriarCasoDeUso(
        CancellationToken cancellationToken,
        string? email = null)
    {
        var repositorioUsuario = new RepositorioUsuarioBuilder();

        if (email != null)
            repositorioUsuario.SetupExisteUsuarioComEmailReturnsTrue(email, cancellationToken);

        return new CadastrarUsuario(
            MapperBuilder.Build(),
            UnitOfWorkBuilder.Build(),
            repositorioUsuario.Build(),
            UsuarioLogadoServicoBuilder.Build(UsuarioBuilder.Build(), cancellationToken),
            EncriptadorSenhaBuilder.Build());
    }
}
