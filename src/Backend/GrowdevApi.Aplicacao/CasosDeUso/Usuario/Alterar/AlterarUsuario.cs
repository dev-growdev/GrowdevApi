using AutoMapper;
using GrowdevApi.Aplicacao.Validadores;
using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Dominio.Interfaces.Repositorios;
using GrowdevApi.Excecao;

namespace GrowdevApi.Aplicacao.CasosDeUso.Usuario.Alterar;

public class AlterarUsuario : IAlterarUsuario
{
    private readonly IRepositorioUsuario _repositorioUsuario;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AlterarUsuario(
        IRepositorioUsuario repositorioUsuario,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _repositorioUsuario = repositorioUsuario;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Executar(RequisicaoAlterarUsuario requisicao, CancellationToken cancellationToken)
    {
        await Validar(requisicao, cancellationToken);

        var usuario = await _repositorioUsuario.PegarUsuarioPorId(Guid.Parse(requisicao.Id!), cancellationToken);
        _mapper.Map(requisicao, usuario);

        await _unitOfWork.SalvarAsync(cancellationToken);
    }

    private async Task Validar(RequisicaoAlterarUsuario requisicao, CancellationToken cancellationToken)
    {
        List<string> mensagensDeErro = [];
        mensagensDeErro.AddRange(await ValidarRequisicao(requisicao, cancellationToken));

        if (mensagensDeErro.Count > 0)
        {
            throw new ErroValidacaoExcecao(mensagensDeErro.Distinct().ToList());
        }

        await ValidarUsuarioAlteracao(requisicao, cancellationToken);
    }

    private async Task<List<string>> ValidarRequisicao(RequisicaoAlterarUsuario requisicao, CancellationToken cancellationToken)
    {
        var validator = new AlterarUsuarioValidador();

        var resultado = await validator.ValidateAsync(requisicao, cancellationToken);

        if (!resultado.IsValid)
        {
            return resultado.Errors.Select(e => e.ErrorMessage).ToList();
        }

        return [];
    }

    private async Task ValidarUsuarioAlteracao(RequisicaoAlterarUsuario requisicao, CancellationToken cancellationToken)
    {
        if (!IdValidador.IdEstaValido(requisicao.Id))
            return;

        var usuario = await _repositorioUsuario.PegarUsuarioPorId(Guid.Parse(requisicao.Id!), cancellationToken);
        if (usuario == null)
            throw new NaoEncontradoExcecao(MensagensExcecao.USUARIO_NAO_ENCONTRADO);

        if (usuario.Email != requisicao.Email && await _repositorioUsuario.ExisteUsuarioComEmail(requisicao.Email!, cancellationToken))
            throw new ErroValidacaoExcecao([MensagensExcecao.EMAIL_JA_EXISTE]);
    }
}