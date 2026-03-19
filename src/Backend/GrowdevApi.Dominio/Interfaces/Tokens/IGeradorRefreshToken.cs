using GrowdevApi.Dominio.Entidades;

namespace GrowdevApi.Dominio.Interfaces.Tokens;

public interface IGeradorRefreshToken
{
    string Gerar();

    bool TokenValido(RefreshToken refreshToken);
}
