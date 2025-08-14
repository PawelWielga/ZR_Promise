using AppAudit.Infrastructure;
using System.Runtime.Versioning;

namespace AppAudit.Cli;

[SupportedOSPlatform("windows")]
internal sealed class AuditConsole(AppOptions opt)
{
    public async Task<int> RunAsync()
    {
        using var cts = new CancellationTokenSource();
        System.Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

        var runner = new AuditRunner(opt);

        do
        {
            try { runner.RunOnce(); }
            catch (Exception ex) { System.Console.Error.WriteLine($"[ERROR] {ex.Message}"); }

            if (opt.Once) break;
            try { await Task.Delay(TimeSpan.FromMinutes(opt.IntervalMinutes), cts.Token); }
            catch (TaskCanceledException) { break; }
        }
        while (!cts.IsCancellationRequested);

        return 0;
    }
}