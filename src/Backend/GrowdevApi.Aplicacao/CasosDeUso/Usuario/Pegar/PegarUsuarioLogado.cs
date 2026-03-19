using AutoMapper;
using GrowdevApi.Comunicacao.Respostas;
using GrowdevApi.Dominio.Interfaces.Servicos;

namespace GrowdevApi.Aplicacao.CasosDeUso.Usuario.Pegar;

public class PegarUsuarioLogado : IPegarUsuarioLogado
{
    private readonly IUsuarioLogadoServico _usuarioLogadoService;
    private readonly IMapper _mapper;

    public PegarUsuarioLogado(
        IUsuarioLogadoServico usuarioLogadoService,
        IMapper mapper)
    {
        _usuarioLogadoService = usuarioLogadoService;
        _mapper = mapper;
    }

    public async Task<RespostaDadosUsuario> Executar(CancellationToken cancellationToken)
    {
        var usuarioLogado = await _usuarioLogadoService.PegarUsuarioLogado(cancellationToken);

        var usuarioMapped = _mapper.Map<RespostaDadosUsuario>(usuarioLogado);

        return usuarioMapped;
    }
}
