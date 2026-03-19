using GrowdevApi.Aplicacao.CasosDeUso.Limpeza.LimpezaLogs;
using Microsoft.Extensions.Configuration;
using Moq;

namespace UnidadeCasosDeUso.Test.Limpeza.LimpezaLogs;

public class LimpezaArquivosLogTest
{
    private static readonly uint dias = 15;
    private static readonly string pastaTemporaria = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

    [Fact]
    public async Task Sucesso_Excluiu_So_Antigos()
    {
        Directory.CreateDirectory(pastaTemporaria);

        var dataAntigo1 = DateTime.Today.AddDays(-(dias + 1));
        var dataAntigo2 = DateTime.Today.AddDays(-(dias + 2));
        var dataRecente1 = DateTime.Today.AddDays(-(dias - 1));
        var dataRecente2 = DateTime.Today.AddDays(-(dias - 2));
        var antigo1 = Path.Combine(pastaTemporaria, $"log{dataAntigo1:yyyyMMdd}.txt");
        var antigo2 = Path.Combine(pastaTemporaria, $"log{dataAntigo2:yyyyMMdd}.txt");
        var recente1 = Path.Combine(pastaTemporaria, $"log{dataRecente1:yyyyMMdd}.txt");
        var recente2 = Path.Combine(pastaTemporaria, $"log{dataRecente2:yyyyMMdd}.txt");
        File.WriteAllText(antigo1, "antigo1");
        File.WriteAllText(antigo2, "antigo2");
        File.WriteAllText(recente1, "recente1");
        File.WriteAllText(recente2, "recente2");

        var casoDeUso = CriarCasoDeUso();

        var excluidos = await casoDeUso.Executar(CancellationToken.None);

        Assert.Contains(antigo1, excluidos);
        Assert.False(File.Exists(antigo1));
        Assert.False(File.Exists(antigo2));
        Assert.True(File.Exists(recente1));
        Assert.True(File.Exists(recente2));
        Assert.DoesNotContain(recente1, excluidos);
        Assert.DoesNotContain(recente2, excluidos);

        File.Delete(recente1);
        File.Delete(recente2);
        Directory.Delete(pastaTemporaria);
    }

    private static ILimpezaArquivosLog CriarCasoDeUso()
    {
        var configMock = new Mock<IConfiguration>();
        var sectionMock = new Mock<IConfigurationSection>();
        sectionMock.Setup(s => s.Value).Returns(dias.ToString());
        configMock.Setup(c => c.GetSection("Limpeza:LimpezaArquivos:LimpezaArquivosLogDias"))
                  .Returns(sectionMock.Object);

        return new LimpezaArquivosLogTestavel(configMock.Object, pastaTemporaria);
    }

    private class LimpezaArquivosLogTestavel : LimpezaArquivosLog
    {
        public LimpezaArquivosLogTestavel(IConfiguration config, string logDir)
            : base(config)
        {
            typeof(LimpezaArquivosLog)
                .GetField("_pastaLogs", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(this, logDir);
        }
    }
}