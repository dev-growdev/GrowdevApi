using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using GrowdevApi.Api.Extensoes;
using GrowdevApi.Api.Filtros;
using GrowdevApi.Api.Token;
using GrowdevApi.Aplicacao.Extensoes;
using GrowdevApi.Dominio.Interfaces.Tokens;
using GrowdevApi.Infraestrutura.Extensoes;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new GrowdevApi.Api.ConversorJson.ConversorString()));

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigurarSwagger();
builder.ConfigurarSerilog();

builder.Services.AddMvc(option => option.Filters.Add(typeof(FiltroExcecao)));

builder.Services.AdicionarAplicacao();
builder.Services.AdicionarInfra(builder.Configuration);

builder.Services.AddScoped<ITokenRecebido, TokenRecebido>();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddHttpContextAccessor();

if (!builder.Configuration.RodandoTesteEmMemoria())
{
    builder.Services.AddHangfire(builder.Configuration);
}

var app = builder.Build();

Log.Information("======== Inicializando API ========");

if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (!builder.Configuration.RodandoTesteEmMemoria())
{
    app.Services.AtivarMigration();
    app.UseHangfire();
}

await app.RunAsync();

return;

public partial class Program
{
    protected Program() { }
}