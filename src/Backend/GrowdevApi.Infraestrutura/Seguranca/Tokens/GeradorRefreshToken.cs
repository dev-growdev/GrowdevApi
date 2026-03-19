using GrowdevApi.Dominio.Entidades;
using GrowdevApi.Dominio.Interfaces.Tokens;

namespace GrowdevApi.Infraestrutura.Seguranca.Tokens;

public class GeradorRefreshToken : IGeradorRefreshToken
{
    private readonly uint _tempoValidadeDias;
    private readonly IGeradorToken _geradorToken;

    public GeradorRefreshToken(
        uint tempoValidadeDias,
        IGeradorToken geradorToken)
    {
        _tempoValidadeDias = tempoValidadeDias;
        _geradorToken = geradorToken;
    }

    public string Gerar() => _geradorToken.GerarToken();

    public bool TokenValido(RefreshToken refreshToken)
    {
        var validoAteDia = refreshToken.DataCriacaoUtc.AddDays(_tempoValidadeDias);
        return validoAteDia >= DateTime.UtcNow;
    }
}
