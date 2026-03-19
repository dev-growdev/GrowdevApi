using GrowdevApi.Dominio.Interfaces.Tokens;
using GrowdevApi.Infraestrutura.Seguranca.Tokens;

namespace TestUtil.Tokens;

public class GeradorTokenUsuarioBuilder
{
    public static IGeradorTokenUsuario Build()
    {
        return new GeradorTokenUsuario(100, "ChaveDeAssinaturaComNoMinimo32Caracteres");
    }
}
