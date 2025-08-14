using AppAudit.Core;
using AppAudit.Core.Abstractions;
using AppAudit.Infrastructure;
using System.Runtime.Versioning;

namespace AppAudit.Cli;

[SupportedOSPlatform("windows")]
internal sealed class ConsoleApp(AppOptions opt, IResultWriter? writer = null)
{
    private readonly IResultWriter _writer = writer ?? new CsvResultWriter();

    public async Task<int> RunAsync()
    {
        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

        var statePath = Path.ChangeExtension(opt.CsvPath, ".state.json");
        var dedup = new FileDeduplicator(statePath);

        do
        {
            var all = ProgramScanner.ScanAll();
            var toWrite = dedup.FilterNew(all).ToList();

            _writer.WriteAll(toWrite, opt.CsvPath);
            dedup.Commit(toWrite);

            Console.WriteLine($"found: {all.Count}, new: {toWrite.Count}, file: {opt.CsvPath}");

            if (opt.Once) break;
            try { await Task.Delay(TimeSpan.FromMinutes(opt.IntervalMinutes), cts.Token); }
            catch (TaskCanceledException) { break; }
        }
        while (!cts.IsCancellationRequested);

        return 0;
    }
}
