using GrowdevApi.Dominio.Entidades;
using GrowdevApi.Dominio.Interfaces.Repositorios;
using GrowdevApi.Dominio.Interfaces.Servicos;
using GrowdevApi.Dominio.Interfaces.Tokens;

namespace GrowdevApi.Infraestrutura.Servicos;

public class UsuarioLogadoServico : IUsuarioLogadoServico
{
    private readonly IValidadorTokenUsuario _validadorTokenUsuario;
    private readonly IRepositorioUsuario _repositorioUsuario;
    private readonly ITokenRecebido _tokenRecebido;

    public UsuarioLogadoServico(
        IValidadorTokenUsuario validadorTokenUsuario,
        IRepositorioUsuario repositorioUsuario,
        ITokenRecebido tokenRecebido)
    {
        _validadorTokenUsuario = validadorTokenUsuario;
        _repositorioUsuario = repositorioUsuario;
        _tokenRecebido = tokenRecebido;
    }

    public async Task<Usuario?> PegarUsuarioLogado(CancellationToken cancellationToken)
    {
        var token = _tokenRecebido.Token();

        var idUsuario = _validadorTokenUsuario.PegarIdUsuario(token);

        return await _repositorioUsuario.PegarUsuarioPorId(idUsuario, cancellationToken);
    }
}
