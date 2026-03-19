using Microsoft.Extensions.Configuration;

namespace GrowdevApi.Infraestrutura.Extensoes;

public static class ExtensaoConfiguracao
{
    public static bool RodandoTesteEmMemoria(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>("TesteEmMemoria");
    }
}
