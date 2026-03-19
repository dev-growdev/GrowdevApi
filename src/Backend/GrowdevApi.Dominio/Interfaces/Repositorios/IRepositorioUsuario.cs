using GrowdevApi.Dominio.Entidades;

namespace GrowdevApi.Dominio.Interfaces.Repositorios;

public interface IRepositorioUsuario
{
    Task<Usuario?> PegarUsuarioPorId(Guid id, CancellationToken cancellationToken);
    Task<bool> ExisteUsuarioComEmail(string email, CancellationToken cancellationToken);
    Task<bool> ExisteUsuarioAtivoComId(Guid id, CancellationToken cancellationToken);
    Task<Usuario?> PegarUsuarioPorEmail(string email, CancellationToken cancellationToken);
    Task Adicionar(Usuario usuario, CancellationToken cancellationToken);
    Task Excluir(Usuario usuario, CancellationToken cancellationToken);
}
