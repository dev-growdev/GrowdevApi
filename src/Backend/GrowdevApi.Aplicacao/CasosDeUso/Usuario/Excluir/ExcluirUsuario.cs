using GrowdevApi.Aplicacao.Validadores;
using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Dominio.Interfaces.Repositorios;
using GrowdevApi.Dominio.Interfaces.Servicos;
using GrowdevApi.Excecao;

namespace GrowdevApi.Aplicacao.CasosDeUso.Usuario.Excluir;

public class ExcluirUsuario : IExcluirUsuario
{
    private readonly IUsuarioLogadoServico _usuarioLogadoServico;
    private readonly IRepositorioUsuario _repositorioUsuario;
    private readonly IUnitOfWork _unitOfWork;

    public ExcluirUsuario(
        IUsuarioLogadoServico usuarioLogadoServico,
        IRepositorioUsuario repositorioUsuario,
        IUnitOfWork unitOfWork)
    {
        _usuarioLogadoServico = usuarioLogadoServico;
        _repositorioUsuario = repositorioUsuario;
        _unitOfWork = unitOfWork;
    }

    public async Task Executar(RequisicaoExcluirUsuario requisicao, CancellationToken cancellationToken)
    {
        var idValido = IdValidador.ValidarId(requisicao.Id);

        var usuarioLogado = await _usuarioLogadoServico.PegarUsuarioLogado(cancellationToken);
        if (usuarioLogado!.Id == idValido)
            throw new ProibidoExcecao(MensagensExcecao.AUTO_EXCLUSAO);

        var usuario = await _repositorioUsuario.PegarUsuarioPorId(idValido, cancellationToken);
        if (usuario == null)
            throw new NaoEncontradoExcecao(MensagensExcecao.USUARIO_NAO_ENCONTRADO);

        await _repositorioUsuario.Excluir(usuario, cancellationToken);

        await _unitOfWork.SalvarAsync(cancellationToken);
    }
}