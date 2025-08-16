namespace AppAudit.Web;

public sealed class WebApiClient(IHttpClientFactory httpFactory)
{
    private readonly HttpClient _http = httpFactory.CreateClient("Api");

    public async Task<T?> GetAsync<T>(string url, CancellationToken ct)
    {
        using var resp = await _http.GetAsync(url, ct);
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<T>(cancellationToken: ct);
    }

    public async Task PostAsync<T>(string url, T body, CancellationToken ct)
    {
        using var resp = await _http.PostAsJsonAsync(url, body, ct);
        resp.EnsureSuccessStatusCode();
    }

    public async Task PutAsync(string url, CancellationToken ct)
    {
        using var resp = await _http.PutAsync(url, null, ct);
        resp.EnsureSuccessStatusCode();
    }

    public async Task PatchAsync(string url, CancellationToken ct)
    {
        using var resp = await _http.PatchAsync(url, null, ct);
        resp.EnsureSuccessStatusCode();
    }
}
