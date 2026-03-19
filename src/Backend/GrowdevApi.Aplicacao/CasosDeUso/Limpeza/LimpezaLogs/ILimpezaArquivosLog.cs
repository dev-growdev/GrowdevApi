namespace GrowdevApi.Aplicacao.CasosDeUso.Limpeza.LimpezaLogs;

public interface ILimpezaArquivosLog
{
    Task<List<string>> Executar(CancellationToken cancellationToken);
}
