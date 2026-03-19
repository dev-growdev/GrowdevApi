using GrowdevApi.Dominio.Entidades;
using GrowdevApi.Dominio.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace GrowdevApi.Infraestrutura.Repositorios
{
    public class RepositorioUsuario : IRepositorioUsuario
    {
        private readonly GrowdevApiDbContext _dbContext;

        public RepositorioUsuario(GrowdevApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Adicionar(Usuario usuario, CancellationToken cancellationToken)
        {
            await _dbContext.Usuarios.AddAsync(usuario, cancellationToken);
        }

        public async Task<bool> ExisteUsuarioAtivoComId(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Usuarios.AnyAsync(u => u.Id == id && u.Status == Dominio.Enums.StatusUsuario.Ativo, cancellationToken);
        }

        public async Task<bool> ExisteUsuarioComEmail(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return await _dbContext.Usuarios.AnyAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<Usuario?> PegarUsuarioPorEmail(string email, CancellationToken cancellationToken)
        {
            return await _dbContext
                .Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(usuario => usuario.Email == email, cancellationToken);
        }

        public async Task<Usuario?> PegarUsuarioPorId(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Usuarios.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task Excluir(Usuario usuario, CancellationToken cancellationToken)
        {
            _dbContext.Usuarios.Remove(usuario);
        }
    }
}
