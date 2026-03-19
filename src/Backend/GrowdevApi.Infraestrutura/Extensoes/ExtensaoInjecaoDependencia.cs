using GrowdevApi.Dominio.Interfaces.Criptografia;
using GrowdevApi.Dominio.Interfaces.DadosIniciais;
using GrowdevApi.Dominio.Interfaces.Repositorios;
using GrowdevApi.Dominio.Interfaces.Servicos;
using GrowdevApi.Dominio.Interfaces.Tokens;
using GrowdevApi.Infraestrutura.Inicializacao;
using GrowdevApi.Infraestrutura.Repositorios;
using GrowdevApi.Infraestrutura.Seguranca.Criptografia;
using GrowdevApi.Infraestrutura.Seguranca.Tokens;
using GrowdevApi.Infraestrutura.Servicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GrowdevApi.Infraestrutura.Extensoes;

public static class ExtensaoInjecaoDependencia
{
    public static void AdicionarInfra(this IServiceCollection services, IConfiguration configuration)
    {
        AdicionarDbContext(services, configuration);
        AdicionarRepositorios(services);
        AddTokens(services, configuration);
        AdicionarServicos(services, configuration);
    }

    public static void AdicionarDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var stringConexao = configuration.GetConnectionString("ConnectionSqlServer");
        services.AddDbContext<GrowdevApiDbContext>(options =>
        {
            options.UseSqlServer(stringConexao, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure();
            });
        });
    }

    public static void AdicionarRepositorios(this IServiceCollection services)
    {
        services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
        services.AddScoped<IRepositorioRefreshToken, RepositorioRefreshToken>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        var jwtChaveAssinatura = configuration.GetValue<string>("JwtUsuario:ChaveAssinatura");
        var jwtEmissor = configuration.GetValue<string>("JwtUsuario:Emissor");
        var jwtTempoValidadeMinutos = configuration.GetValue<uint>("JwtUsuario:TempoValidadeMinutos");
        services.AddScoped<IGeradorTokenUsuario>(_ => new GeradorTokenUsuario(jwtTempoValidadeMinutos, jwtChaveAssinatura!));
        services.AddScoped<IValidadorTokenUsuario>(_ => new ValidadorTokenUsuario(jwtChaveAssinatura!));

        var tempoValidadeDiasRefreshToken = configuration.GetValue<uint>("RefreshToken:TempoValidadeDias");
        services.AddScoped<IGeradorRefreshToken>(_ => new GeradorRefreshToken(tempoValidadeDiasRefreshToken, new GeradorToken()));
    }

    public static void AdicionarServicos(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDadosIniciais, DadosIniciais>();
        services.AddScoped<IEncriptadorSenha, EncriptadorBCrypt>();
        services.AddScoped<IUsuarioLogadoServico, UsuarioLogadoServico>();
    }
}
