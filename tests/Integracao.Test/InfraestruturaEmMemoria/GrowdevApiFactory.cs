using GrowdevApi.Dominio.Interfaces.DadosIniciais;
using GrowdevApi.Infraestrutura.Inicializacao;
using GrowdevApi.Infraestrutura.Repositorios;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Integracao.Test.InfraestruturaEmMemoria;

public class GrowdevApiFactory : WebApplicationFactory<Program>
{
    protected Dictionary<string, object> _entidadesCriadas = [];

    public Dictionary<string, object> EntidadesCriadas() => _entidadesCriadas;

    public IConfiguration? Configuration { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureAppConfiguration((context, configBuilder) =>
            {
                Configuration = configBuilder.Build();
            })
            .ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<GrowdevApiDbContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<GrowdevApiDbContext>(options =>
                {
                    options.UseInMemoryDatabase("GrowdevApiTestDb");
                    options.UseInternalServiceProvider(provider);
                });

                using var scope = services.BuildServiceProvider().CreateScope();

                InicializarDados(scope);
            });
    }

    private void InicializarDados(IServiceScope scope)
    {
        var dadosIniciais = scope.ServiceProvider.GetRequiredService<IDadosIniciais>();
        dadosIniciais.Cadastrar();

        _entidadesCriadas["UsuarioAdministrador"] = DadosIniciais.UsuarioAdministrador;

        //var dbContext = scope.ServiceProvider.GetRequiredService<GrowdevApiDbContext>();
    }
}
