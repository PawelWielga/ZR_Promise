using AppAudit.Abstractions;
using AppAudit.Infrastructure;
using System.Runtime.Versioning;

namespace AppAudit.Cli;

[SupportedOSPlatform("windows")]
internal sealed class AuditRunner(AppOptions opt, IResultWriter writer, IDeduplicator dedup)
{
    public void RunOnce()
    {
        var all = ProgramScanner.ScanAll();
        var toWrite = dedup.FilterNew(all).ToList();

        var now = DateTimeOffset.UtcNow;
        var scanId = Guid.NewGuid().ToString("N");

        writer.WriteAll(toWrite, opt.CsvPath, now, scanId);
        dedup.Commit(toWrite);

        Console.WriteLine($"found: {all.Count}, new: {toWrite.Count}, file: {opt.CsvPath}");
    }
}
