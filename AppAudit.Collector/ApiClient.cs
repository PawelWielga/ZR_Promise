using AppAudit.Contracts;
using System.Net.Http.Json;

namespace AppAudit.Collector;

public sealed class ApiClient(HttpClient http)
{
    public async Task PostAsync(ProgramRecord record, CancellationToken ct)
    {
        for (int attempt = 0; ; attempt++)
        {
            try
            {
                using var resp = await http.PostAsJsonAsync("/api/programs", record, ct);
                resp.EnsureSuccessStatusCode();
                return;
            }
            catch when (attempt < 3)
            {
                await Task.Delay(TimeSpan.FromSeconds(2 << attempt), ct);
            }
        }
    }
}