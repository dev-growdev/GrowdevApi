using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GrowdevApi.Infraestrutura.Seguranca.Tokens;

public class ChaveSeguraJwt
{
    protected static SymmetricSecurityKey ChaveSegura(string chaveAssinatura)
    {
        var bytes = Encoding.UTF8.GetBytes(chaveAssinatura);

        return new SymmetricSecurityKey(bytes);
    }
}
