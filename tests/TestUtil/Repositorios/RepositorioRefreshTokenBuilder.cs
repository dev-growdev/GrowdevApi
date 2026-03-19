using GrowdevApi.Dominio.Entidades;
using GrowdevApi.Dominio.Interfaces.Repositorios;
using Moq;

namespace TestUtil.Repositorios;

public class RepositorioRefreshTokenBuilder
{
    private readonly Mock<IRepositorioRefreshToken> _refreshTokenRepository;

    public RepositorioRefreshTokenBuilder()
    {
        _refreshTokenRepository = new Mock<IRepositorioRefreshToken>();
    }

    public IRepositorioRefreshToken Build()
    {
        return _refreshTokenRepository.Object;
    }

    public void SetupPegarRefreshTokenReturnsRefreshToken(string stringRefreshToken, RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        _refreshTokenRepository.Setup(repository => repository.PegarRefreshToken(stringRefreshToken, cancellationToken)).ReturnsAsync(refreshToken);
    }
}