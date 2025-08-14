using AppAudit.Cli.Options;
using AppAudit.Infrastructure.Deduplication;
using AppAudit.Infrastructure.Scanners;
using AppAudit.Infrastructure.Writers;
using System.Runtime.Versioning;

namespace AppAudit.Cli.Audit;

[SupportedOSPlatform("windows")]
internal sealed class AuditConsole(AppOptions opt)
{
    public async Task<int> RunAsync()
    {
        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

        var statePath = Path.ChangeExtension(opt.CsvPath, ".state.json");
        var scanner = new RegistryProgramScanner();
        IResultWriter writer = opt.Destination == SaveDestination.Api
            ? new ApiResultWriter(opt.ApiUrl!, opt.DeviceId)
            : new CsvResultWriter(opt.CsvPath);
        var dedup = new FileDeduplicator(statePath);
        var runner = new AuditRunner(opt, scanner, writer, dedup);

        await AuditLoop.RunAsync(
            runner.RunOnce,
            opt.Once,
            TimeSpan.FromMinutes(opt.IntervalMinutes),
            cts.Token);

        return 0;
    }
}
