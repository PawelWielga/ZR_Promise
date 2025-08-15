using AppAudit.Collector;
using AppAudit.Contracts;
using Microsoft.Win32;

internal class Program
{
    static async Task<int> Main(string[] args)
    {
        var opts = Options.Parse(args);
        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

        var handler = new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(5) };
        using var http = new HttpClient(handler) { BaseAddress = opts.ApiBase, Timeout = TimeSpan.FromSeconds(15) };
        var api = new ApiClient(http);

        Console.WriteLine($"[Collector] Start. API: {opts.ApiBase}. Interval: {opts.IntervalMinutes} min");

        while (!cts.IsCancellationRequested)
        {
            try
            {
                var now = DateTimeOffset.Now;
                int sent = 0;

                var scanned = RegistryScanner.Scan().ToList();

                static string N(string? s) => (s ?? "").Trim().ToLowerInvariant();
                var groups = scanned.GroupBy(p => (N(p.Name), N(p.Version), N(p.Publisher)));

                foreach (var g in groups)
                {
                    // Wybierz najlepszy wpis z grupy
                    var best = g
                        .OrderByDescending(p => !string.IsNullOrWhiteSpace(p.UninstallString))
                        .ThenByDescending(p => !string.IsNullOrWhiteSpace(p.InstallLocation))
                        .ThenByDescending(p => p.Hive == RegistryHive.LocalMachine)
                        .ThenByDescending(p => p.View == RegistryView.Registry64)
                        .First();

                    var id = ProgramId.FromProduct(best.Name, best.Version, best.Publisher);

                    var dto = new ProgramRecord(
                        ProgramId: id,
                        Name: best.Name,
                        Version: best.Version,
                        Publisher: best.Publisher,
                        DiscoveredAt: now,
                        RequiresLicense: false,
                        LicenseKey: null
                    );

                    await api.PostAsync(dto, cts.Token);
                    sent++;
                }

                Console.WriteLine($"[Collector] {now:yyyy-MM-dd HH:mm:ss} wysłano (po filtrach/dedupe): {sent}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Collector][ERR] " + ex.Message);
            }

            await Task.Delay(TimeSpan.FromMinutes(opts.IntervalMinutes), cts.Token);
        }

        Console.WriteLine("[Collector] Stop.");
        return 0;
    }
}
