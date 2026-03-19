using Integracao.Test.Helpers;

namespace Integracao.Test.InfraestruturaEmMemoria;

public class GrowdevApiClassFixture
{
    protected HttpClient _httpClient;
    protected GrowdevApiFactory _factory;
    protected HttpHelper _httpHelper;
    protected CadastroHelper _cadastroHelper;

    public GrowdevApiClassFixture()
    {
        _factory = TestServerSingleton.Instance;
        _httpClient = _factory.CreateClient();
        _httpHelper = new HttpHelper(_httpClient);
        _cadastroHelper = new CadastroHelper(_httpHelper);
    }
}
