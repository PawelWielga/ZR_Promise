using AppAudit.Cli.Options;
using AppAudit.Infrastructure.Deduplication;
using AppAudit.Infrastructure.Scanners;
using AppAudit.Infrastructure.Writers;
using System.Runtime.Versioning;

namespace AppAudit.Cli.Audit;

[SupportedOSPlatform("windows")]
internal sealed class AuditRunner(AppOptions opt, IProgramScanner scanner, IResultWriter writer, IDeduplicator dedup)
{
    public void RunOnce()
    {
        var all = scanner.ScanAll();
        var toWrite = dedup.FilterNew(all).ToList();

        var now = DateTimeOffset.UtcNow;
        var scanId = Guid.NewGuid().ToString("N");

        writer.WriteAll(toWrite, now, scanId);
        dedup.Commit(toWrite);

        var destMsg = opt.Destination == SaveDestination.Api
            ? $"api: {opt.ApiUrl}"
            : $"file: {opt.CsvPath}";
        Console.WriteLine($"found: {all.Count}, new: {toWrite.Count}, {destMsg}");
    }
}
