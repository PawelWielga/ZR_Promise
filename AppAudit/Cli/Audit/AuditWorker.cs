using AppAudit.Cli.Options;
using AppAudit.Infrastructure.Deduplication;
using AppAudit.Infrastructure.Scanners;
using AppAudit.Infrastructure.Writers;
using Microsoft.Extensions.Hosting;
using System.Runtime.Versioning;

namespace AppAudit.Cli.Audit;

[SupportedOSPlatform("windows")]
internal sealed class AuditWorker(AppOptions opt, IProgramScanner scanner, IResultWriter writer, IDeduplicator dedup) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var runner = new AuditRunner(opt, scanner, writer, dedup);

        return AuditLoop.RunAsync(
            runner.RunOnce,
            false,
            TimeSpan.FromMinutes(opt.IntervalMinutes),
            stoppingToken);
    }
}
