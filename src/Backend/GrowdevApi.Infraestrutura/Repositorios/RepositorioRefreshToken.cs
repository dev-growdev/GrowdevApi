using GrowdevApi.Dominio.Entidades;
using GrowdevApi.Dominio.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace GrowdevApi.Infraestrutura.Repositorios;

public class RepositorioRefreshToken : IRepositorioRefreshToken
{
    private readonly GrowdevApiDbContext _dbContext;

    public RepositorioRefreshToken(GrowdevApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RefreshToken?> PegarRefreshToken(string refreshToken, CancellationToken cancellationToken)
    {
        return await _dbContext
                .RefreshTokens
                .AsNoTracking()
                .Include(token => token.Usuario)
                .FirstOrDefaultAsync(token => token.Valor.Equals(refreshToken)
                    && (token.Usuario.Status == Dominio.Enums.StatusUsuario.Ativo), cancellationToken);
    }

    public async Task SalvarNovoRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        var tokens = _dbContext.RefreshTokens.Where(token => token.IdUsuario.Equals(refreshToken.IdUsuario));
        _dbContext.RefreshTokens.RemoveRange(tokens);
        await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
    }
}
