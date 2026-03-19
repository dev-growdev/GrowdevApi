using GrowdevApi.Dominio.Entidades;

namespace GrowdevApi.Dominio.Interfaces.Servicos;

public interface IUsuarioLogadoServico
{
    Task<Usuario?> PegarUsuarioLogado(CancellationToken cancellationToken);
}
