using AppAudit.Core;
using AppAudit.Infrastructure;
using System.Runtime.Versioning;

namespace AppAudit.Console;

[SupportedOSPlatform("windows")]
internal sealed class ConsoleApp(AppOptions opt)
{
    public async Task<int> RunAsync()
    {
        using var cts = new CancellationTokenSource();
        System.Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

        var scanner = new ProgramScanner();
        var entries = ProgramScanner.ScanAll();

        System.Console.WriteLine($"found: {entries.Count}");
        foreach (var e in entries.Take(10)) // Wyświetl tylko 10 do testów
            System.Console.WriteLine($"- {e.DisplayName} {e.DisplayVersion} ({e.RegistryHive} {e.RegistryView})");

        if (opt.Once) return 0;
        try { await Task.Delay(TimeSpan.FromMinutes(opt.IntervalMinutes), cts.Token); } catch (TaskCanceledException) { }
        return 0;
    }
}