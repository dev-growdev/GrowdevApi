using GrowdevApi.Infraestrutura.Seguranca.Criptografia;

namespace TestUtil.Criptografia;

public class EncriptadorSenhaBuilder
{
    public static EncriptadorBCrypt Build()
    {
        return new EncriptadorBCrypt();
    }
}
