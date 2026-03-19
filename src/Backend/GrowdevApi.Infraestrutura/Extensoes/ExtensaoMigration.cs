using GrowdevApi.Dominio.Interfaces.DadosIniciais;
using GrowdevApi.Infraestrutura.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GrowdevApi.Infraestrutura.Extensoes;

public static class ExtensaoMigration
{
    public static void AtivarMigration(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GrowdevApiDbContext>();
        dbContext.Database.Migrate();
        var dadosIniciais = scope.ServiceProvider.GetRequiredService<IDadosIniciais>();
        dadosIniciais.Cadastrar();
    }
}
