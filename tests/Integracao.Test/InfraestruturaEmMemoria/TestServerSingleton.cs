namespace Integracao.Test.InfraestruturaEmMemoria;

public static class TestServerSingleton
{
    private static readonly Lazy<GrowdevApiFactory> _instance =
        new(() =>
        {
            var factory = new GrowdevApiFactory();
            factory.Server.PreserveExecutionContext = true;
            return factory;
        });

    public static GrowdevApiFactory Instance => _instance.Value;
}
