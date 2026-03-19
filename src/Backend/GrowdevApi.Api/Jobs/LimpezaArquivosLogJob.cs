using GrowdevApi.Aplicacao.CasosDeUso.Limpeza.LimpezaLogs;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GrowdevApi.Api.Jobs;

public class LimpezaArquivosLogJob
{
    private readonly IServiceProvider _serviceProvider;
    private readonly uint _limpezaArquivosLogDias;

    public LimpezaArquivosLogJob(
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _limpezaArquivosLogDias = configuration
            .GetSection("Limpeza:LimpezaArquivos:LimpezaArquivosLogDias")
            .Get<uint>();
    }

    public async Task ExecutarAsync([FromServices] PerformContext context)
    {
        using var scope = _serviceProvider.CreateScope();
        var limpezaLogsAntigos = scope.ServiceProvider.GetRequiredService<ILimpezaArquivosLog>();

        LogaMensagem($"Procurando arquivos de log com {_limpezaArquivosLogDias} dias ou mais para limpeza", context);

        var arquivos = await limpezaLogsAntigos.Executar(CancellationToken.None);

        foreach (var arquivo in arquivos)
        {
            LogaMensagem($"Removido arquivo {arquivo}", context);
        }

        LogaMensagem($"Removidos {arquivos.Count} arquivos de log", context);
    }

    private void LogaMensagem(string mensagem, PerformContext context)
    {
        Log.Information($"{this.GetType().Name} - {mensagem}");
        context.WriteLine($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - {mensagem}");
    }
}