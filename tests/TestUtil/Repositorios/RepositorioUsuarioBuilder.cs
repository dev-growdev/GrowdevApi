using GrowdevApi.Dominio.Entidades;
using GrowdevApi.Dominio.Interfaces.Repositorios;
using Moq;

namespace TestUtil.Repositorios;

public class RepositorioUsuarioBuilder
{
    private readonly Mock<IRepositorioUsuario> _usuarioRepository;

    public RepositorioUsuarioBuilder()
    {
        _usuarioRepository = new Mock<IRepositorioUsuario>();
    }
    public IRepositorioUsuario Build()
    {
        return _usuarioRepository.Object;
    }

    public void SetupExisteUsuarioComEmailReturnsTrue(string email, CancellationToken cancellationToken)
    {
        _usuarioRepository.Setup(repository => repository.ExisteUsuarioComEmail(email, cancellationToken)).ReturnsAsync(true);
    }

    public void SetupPegarUsuarioPorIdReturnsUsuario(Usuario usuario, CancellationToken cancellationToken)
    {
        _usuarioRepository.Setup(repository => repository.PegarUsuarioPorId(usuario.Id, cancellationToken)).ReturnsAsync(usuario);
    }

    public void SetupPegarUsuarioPorEmailReturnsUsuario(Usuario usuario, CancellationToken cancellationToken)
    {
        _usuarioRepository.Setup(repository => repository.PegarUsuarioPorEmail(usuario.Email, cancellationToken)).ReturnsAsync(usuario);
    }
}
