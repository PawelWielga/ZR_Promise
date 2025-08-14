using AppAudit.Abstractions;
using AppAudit.Infrastructure;
using System.Runtime.Versioning;

namespace AppAudit.Cli;

[SupportedOSPlatform("windows")]
internal sealed class AuditRunner
{
    private readonly AppOptions _opt;
    private readonly IResultWriter _writer;
    private readonly FileDeduplicator _dedup;

    public AuditRunner(AppOptions opt, IResultWriter? writer = null)
    {
        _opt = opt;
        _writer = writer ?? new CsvResultWriter();
        var statePath = Path.ChangeExtension(_opt.CsvPath, ".state.json");
        _dedup = new FileDeduplicator(statePath);
    }

    public void RunOnce()
    {
        var all = ProgramScanner.ScanAll();
        var toWrite = _dedup.FilterNew(all).ToList();

        var now = DateTimeOffset.UtcNow;
        var scanId = Guid.NewGuid().ToString("N");

        _writer.WriteAll(toWrite, _opt.CsvPath, now, scanId);
        _dedup.Commit(toWrite);

        System.Console.WriteLine($"found: {all.Count}, new: {toWrite.Count}, file: {_opt.CsvPath}");
    }
}
