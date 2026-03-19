using AutoMapper;
using GrowdevApi.Aplicacao.Validadores;
using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Comunicacao.Respostas;
using GrowdevApi.Dominio.Interfaces.Repositorios;
using GrowdevApi.Excecao;

namespace GrowdevApi.Aplicacao.CasosDeUso.Usuario.Pegar;

public class PegarUsuarioPorId : IPegarUsuarioPorId
{
    private readonly IRepositorioUsuario _repositorioUsuario;
    private readonly IMapper _mapper;

    public PegarUsuarioPorId(
        IRepositorioUsuario usuarioRepositorio,
        IMapper mapper)
    {
        _repositorioUsuario = usuarioRepositorio;
        _mapper = mapper;
    }

    public async Task<RespostaDadosUsuario> Executar(RequisicaoPegarUsuario requisicao, CancellationToken cancellationToken)
    {
        var idValido = IdValidador.ValidarId(requisicao.Id);

        var usuario = await _repositorioUsuario.PegarUsuarioPorId(idValido, cancellationToken);
        if (usuario == null)
            throw new NaoEncontradoExcecao(MensagensExcecao.USUARIO_NAO_ENCONTRADO);

        return _mapper.Map<RespostaDadosUsuario>(usuario);
    }
}