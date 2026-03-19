using Microsoft.Extensions.Configuration;

namespace GrowdevApi.Aplicacao.CasosDeUso.Limpeza.LimpezaLogs;

public class LimpezaArquivosLog : ILimpezaArquivosLog
{
    private readonly string _pastaLogs;
    private readonly uint _limpezaArquivosLogDias;

    public LimpezaArquivosLog(IConfiguration configuration)
    {
        _pastaLogs = Path.Combine(AppContext.BaseDirectory, "logs");
        _limpezaArquivosLogDias = configuration
            .GetSection("Limpeza:LimpezaArquivos:LimpezaArquivosLogDias")
            .Get<uint>();
    }

    public Task<List<string>> Executar(CancellationToken cancellationToken)
    {
        if (!Directory.Exists(_pastaLogs))
            return Task.FromResult(new List<string>());

        var arquivos = Directory.GetFiles(_pastaLogs, "log*.txt");

        List<string> excluir = [];
        List<string> excluidos = [];

        var dataLimite = DateTime.Today.AddDays(-_limpezaArquivosLogDias);

        foreach (var arquivo in arquivos)
        {
            var nomeArquivo = Path.GetFileNameWithoutExtension(arquivo);
            if (nomeArquivo.Length != 11)
                continue;

            var dataStr = nomeArquivo.Substring(3, 8);
            if (DateTime.TryParseExact(dataStr, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var dataArquivo))
            {
                if (dataArquivo <= dataLimite)
                {
                    excluir.Add(arquivo);
                    excluidos.Add(arquivo);
                }
            }
        }

        foreach (var arquivo in excluir)
        {
            try { File.Delete(arquivo); } catch { excluidos.Remove(arquivo); }
        }

        return Task.FromResult(excluidos);
    }
}
