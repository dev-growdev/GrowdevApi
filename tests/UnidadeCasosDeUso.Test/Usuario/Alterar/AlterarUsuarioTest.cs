using GrowdevApi.Aplicacao.CasosDeUso.Usuario.Alterar;
using GrowdevApi.Dominio.Enums;
using GrowdevApi.Excecao;
using TestUtil.AutoMapper;
using TestUtil.Entidades;
using TestUtil.Repositorios;
using TestUtil.Requisicoes;

namespace UnidadeCasosDeUso.Test.Usuario.Alterar;

public class AlterarUsuarioTest
{
    [Fact]
    public async Task Sucesso()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var usuarioAlteracao = UsuarioBuilder.Build();

        var requisicao = RequisicaoAlterarUsuarioBuilder.Build(usuarioAlteracao);
        requisicao.Nome += " da Silva";

        var casoDeUso = CriarCasoDeUso(cancellationToken: cancellationToken, usuarioAlteracao: usuarioAlteracao);

        async Task func() => await casoDeUso.Executar(requisicao, cancellationToken);

        var exception = await Record.ExceptionAsync(func);
        Assert.Null(exception);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("asdfasdf")]
    [InlineData("b9fc55af-e38u-4852-b4f5-ad6b1277472d")]
    public async Task Erro_Id_Invalido(string id)
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var usuarioAlteracao = UsuarioBuilder.Build();

        var requisicao = RequisicaoAlterarUsuarioBuilder.Build();
        requisicao.Id = id;

        var casoDeUso = CriarCasoDeUso(cancellationToken: cancellationToken, usuarioAlteracao: usuarioAlteracao);

        async Task func() => await casoDeUso.Executar(requisicao, cancellationToken);

        var ex = await Assert.ThrowsAsync<ErroValidacaoExcecao>(func);
        Assert.NotNull(ex);
        var mensagensDeErro = ex!.PegarMensagensDeErro();
        Assert.Single(mensagensDeErro);
        Assert.Contains(MensagensExcecao.ID_INVALIDO, mensagensDeErro);
    }

    [Fact]
    public async Task Erro_Usuario_Nao_Encontrado()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var usuarioLogado = UsuarioBuilder.Build();

        var requisicao = RequisicaoAlterarUsuarioBuilder.Build();

        var casoDeUso = CriarCasoDeUso(cancellationToken: cancellationToken);

        async Task func() => await casoDeUso.Executar(requisicao, cancellationToken);

        var ex = await Assert.ThrowsAsync<NaoEncontradoExcecao>(func);
        Assert.NotNull(ex);
        var mensagensDeErro = ex!.PegarMensagensDeErro();
        Assert.Single(mensagensDeErro);
        Assert.Contains(MensagensExcecao.USUARIO_NAO_ENCONTRADO, mensagensDeErro);
    }

    [Fact]
    public async Task Erro_Email_Ja_Existe()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var usuarioAlteracao = UsuarioBuilder.Build();

        var requisicao = RequisicaoAlterarUsuarioBuilder.Build(usuarioAlteracao);
        requisicao.Email = "novo.email@email.com";

        var casoDeUso = CriarCasoDeUso(cancellationToken: cancellationToken, usuarioAlteracao: usuarioAlteracao, email: requisicao.Email);

        async Task func() => await casoDeUso.Executar(requisicao, cancellationToken);

        var ex = await Assert.ThrowsAsync<ErroValidacaoExcecao>(func);
        Assert.NotNull(ex);
        var mensagensDeErro = ex!.PegarMensagensDeErro();
        Assert.Single(mensagensDeErro);
        Assert.Contains(MensagensExcecao.EMAIL_JA_EXISTE, mensagensDeErro);
    }

    [Theory]
    [InlineData("Suspenso")]
    [InlineData("dfghjfgh")]
    [InlineData("Excluido")]
    public async Task Erro_Alterar_Status_Invalido(string? status)
    {
        var cancellationToken = new CancellationTokenSource().Token;

        var usuarioAlteracao = UsuarioBuilder.Build();

        var requisicao = RequisicaoAlterarUsuarioBuilder.Build(usuarioAlteracao);
        requisicao.Status = status!;

        var casoDeUso = CriarCasoDeUso(cancellationToken: cancellationToken, usuarioAlteracao: usuarioAlteracao);

        async Task func() => await casoDeUso.Executar(requisicao, cancellationToken);

        var ex = await Assert.ThrowsAsync<ErroValidacaoExcecao>(func);
        Assert.NotNull(ex);
        var mensagensDeErro = ex!.PegarMensagensDeErro();
        Assert.Single(mensagensDeErro);
        var mensagemEsperada = MensagensExcecao.STATUS_INVALIDO.Replace("{ValoresPossiveis}",
            EnumUtil.PegarNomesEnumSeparadosPorVirgula<StatusUsuario>(["Indefinido"]));
        Assert.Contains(mensagemEsperada, mensagensDeErro);
    }

    private static AlterarUsuario CriarCasoDeUso(
        CancellationToken cancellationToken,
        GrowdevApi.Dominio.Entidades.Usuario? usuarioAlteracao = null,
        string? email = null)
    {
        var usuarioRepository = new RepositorioUsuarioBuilder();

        if (usuarioAlteracao != null)
            usuarioRepository.SetupPegarUsuarioPorIdReturnsUsuario(usuarioAlteracao, cancellationToken);

        if (email != null)
            usuarioRepository.SetupExisteUsuarioComEmailReturnsTrue(email, cancellationToken);

        return new AlterarUsuario(
            usuarioRepository.Build(),
            UnitOfWorkBuilder.Build(),
            MapperBuilder.Build());
    }
}
