using AppAudit.Infrastructure;
using System.Runtime.Versioning;

namespace AppAudit.Cli;

[SupportedOSPlatform("windows")]
internal sealed class AuditConsole(AppOptions opt)
{
    public async Task<int> RunAsync()
    {
        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

        var runner = new AuditRunner(opt);

        await AuditLoop.RunAsync(
            runner.RunOnce,
            opt.Once,
            TimeSpan.FromMinutes(opt.IntervalMinutes),
            cts.Token);

        return 0;
    }
}