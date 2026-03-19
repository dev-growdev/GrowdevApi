using GrowdevApi.Dominio.Entidades;

namespace GrowdevApi.Dominio.Interfaces.Repositorios;

public interface IRepositorioRefreshToken
{
    Task<RefreshToken?> PegarRefreshToken(string refreshToken, CancellationToken cancellationToken);
    Task SalvarNovoRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken);
}
