using AppAudit.Core;
using Microsoft.Extensions.Hosting;
using System.Runtime.Versioning;

namespace AppAudit.Cli;

[SupportedOSPlatform("windows")]
internal sealed class AuditWorker(AppOptions opt) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var runner = new AuditRunner(opt);

        while (!stoppingToken.IsCancellationRequested)
        {
            try { runner.RunOnce(); }
            catch (Exception ex) { Console.Error.WriteLine($"[ERROR] {ex.Message}"); }

            try { await Task.Delay(TimeSpan.FromMinutes(opt.IntervalMinutes), stoppingToken); }
            catch (TaskCanceledException) { break; }
        }
    }
}