using AppAudit.Abstractions;
using AppAudit.Infrastructure;
using Microsoft.Extensions.Hosting;
using System.Runtime.Versioning;

namespace AppAudit.Cli;

[SupportedOSPlatform("windows")]
internal sealed class AuditWorker(AppOptions opt, IResultWriter writer, IDeduplicator dedup) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var runner = new AuditRunner(opt, writer, dedup);

        return AuditLoop.RunAsync(
            runner.RunOnce,
            false,
            TimeSpan.FromMinutes(opt.IntervalMinutes),
            stoppingToken);
    }
}