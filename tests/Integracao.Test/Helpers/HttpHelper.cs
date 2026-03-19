using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Integracao.Test.Helpers;

public class HttpHelper
{
    private readonly HttpClient _httpClient;

    public HttpHelper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> DoPost(string url, CancellationToken cancellationToken, object requisicao, string token = "")
    {
        LimpaHeaders();

        AddAuthorization(token);
        return await _httpClient.PostAsJsonAsync(url, requisicao, cancellationToken);
    }

    public async Task<HttpResponseMessage> DoGet(string url, CancellationToken cancellationToken, string token = "")
    {
        LimpaHeaders();

        AddAuthorization(token);
        return await _httpClient.GetAsync(url, cancellationToken);
    }

    public async Task<HttpResponseMessage> DoDelete(string url, CancellationToken cancellationToken, string token = "")
    {
        LimpaHeaders();

        AddAuthorization(token);
        return await _httpClient.DeleteAsync(url, cancellationToken);
    }

    public async Task<HttpResponseMessage> DoPut(string url, CancellationToken cancellationToken, object requisicao, string token = "")
    {
        LimpaHeaders();

        AddAuthorization(token);
        return await _httpClient.PutAsJsonAsync(url, requisicao, cancellationToken);
    }

    private void AddAuthorization(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public void AddHeader(string key, string value)
    {
        if (_httpClient.DefaultRequestHeaders.Contains(key))
            _httpClient.DefaultRequestHeaders.Remove(key);

        _httpClient.DefaultRequestHeaders.Add(key, value);
    }

    public void LimpaHeaders()
    {
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}
