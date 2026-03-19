using Asp.Versioning.ApiExplorer;
using GrowdevApi.Api.Jobs;
using Hangfire;
using Hangfire.Console;
using Microsoft.OpenApi.Models;
using Serilog;

namespace GrowdevApi.Api.Extensoes;

public static class ExtensaoBuilder
{
    private static bool _consoleInitialized = false;

    public static void ConfigurarSwagger(this IServiceCollection services)
    {
        _ = services.AddSwaggerGen(options =>
        {
            var provider = services.BuildServiceProvider()
                .GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = $"Growdev API {description.ApiVersion}",
                    Version = description.ApiVersion.ToString()
                });
            }

            const string nomeEsquema = "Bearer";

            options.AddSecurityDefinition(nomeEsquema, new OpenApiSecurityScheme
            {
                Description = @"Cabeçalho de autorização JWT usando o esquema Bearer.
                    Insira 'Bearer' [espaço] e seu token no início do texto abaixo.
                    Exemplo: 'Bearer 123abcde'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = nomeEsquema
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = nomeEsquema
                        },
                        Scheme = "oauth2",
                        Name = nomeEsquema,
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });

            var xmlFiles = new[]
            {
                "GrowdevApi.Api.xml",
                "GrowdevApi.Comunicacao.xml"
            };

            foreach (var xmlFile in xmlFiles)
            {
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                }
            }

        });

    }

    public static void ConfigurarSerilog(this WebApplicationBuilder builder)
    {
        var strLogLevel = builder.Configuration.GetValue<string>("Logging:LogLevel:Default");
        var enumLogEventLevel = Enum.TryParse<Serilog.Events.LogEventLevel>(strLogLevel, true, out var parsedLogEventLevel)
            ? parsedLogEventLevel
            : Serilog.Events.LogEventLevel.Information;
        builder.Host.UseSerilog((context, configuration) =>
            configuration.WriteTo.File(Path.Combine(AppContext.BaseDirectory, "logs", "log.txt"),
                enumLogEventLevel,
                rollingInterval: RollingInterval.Day,
                shared: true));
    }

    public static void AddHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ConnectionSqlServer")!;
        services.AddHangfire(config =>
        {
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                  .UseSimpleAssemblyNameTypeSerializer()
                  .UseRecommendedSerializerSettings()
                  .UseSqlServerStorage(connectionString);

            if (!_consoleInitialized)
            {
                config.UseConsole();
                _consoleInitialized = true;
            }
        });

        services.AddHangfireServer();
    }

    public static void UseHangfire(this IApplicationBuilder app)
    {
        app.UseHangfireDashboard("/jobs");

        RecurringJob.AddOrUpdate<LimpezaArquivosLogJob>(
            "limpeza-arquivos-log",
            job => job.ExecutarAsync(null!),
            "0 2 * * *");
    }
}
